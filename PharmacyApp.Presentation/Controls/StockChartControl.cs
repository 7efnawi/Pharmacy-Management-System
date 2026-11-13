// PharmacyApp.Presentation/Controls/StockChartControl.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Presentation.Controls
{
    public class StockChartControl : UserControl
    {
        private List<Medicine> _medicines = new();
        private const int BarWidth = 40;
        private const int BarGap = 20;
        private const int PaddingLeft = 50;
        private const int PaddingBottom = 50;
        private const int PaddingTop = 20;

        public StockChartControl()
        {
            DoubleBuffered = true;  // Eliminates flicker on repaint
            ResizeRedraw = true;    // Redraws automatically when control resizes
        }

        /// <summary>
        /// Sets the data source for the chart and triggers a repaint.
        /// Call this whenever medicine data changes (after save, edit, or sale).
        /// </summary>
        public void SetData(IEnumerable<Medicine> medicines)
        {
            _medicines = medicines
                .OrderBy(m => m.StockQuantity)
                .Take(8)   // Show up to 8 medicines — prevents chart overflow
                .ToList();
            Invalidate();  // Triggers OnPaint → redraws chart with new data
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_medicines.Count == 0) return;

            var g = e.Graphics;
            g.Clear(Color.White);

            int maxStock = _medicines.Max(m => Math.Max(m.StockQuantity, m.MinStockLevel));
            if (maxStock == 0) maxStock = 1;

            int chartHeight = Height - PaddingBottom - PaddingTop;
            int chartWidth  = Width - PaddingLeft;

            // Draw chart title
            using var titleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            g.DrawString("Stock Level Overview", titleFont, Brushes.DimGray, PaddingLeft, 4);

            for (int i = 0; i < _medicines.Count; i++)
            {
                var med = _medicines[i];
                int x = PaddingLeft + i * (BarWidth + BarGap);
                if (x + BarWidth > Width - 10) break;  // Guard against overflow

                // Determine bar colour based on stock health
                Color barColor = med.StockQuantity >= med.MinStockLevel
                    ? (med.StockQuantity <= med.MinStockLevel * 1.5 ? Color.Orange : Color.SeaGreen)
                    : Color.Crimson;

                // Calculate bar height proportional to chart area
                int barHeight = (int)((double)med.StockQuantity / maxStock * chartHeight);
                int barTop = Height - PaddingBottom - barHeight;

                // Draw filled bar
                using var barBrush = new SolidBrush(barColor);
                g.FillRectangle(barBrush, x, barTop, BarWidth, barHeight);

                // Draw bar outline
                g.DrawRectangle(Pens.DimGray, x, barTop, BarWidth, barHeight);

                // Draw minimum threshold dashed line
                if (med.MinStockLevel > 0)
                {
                    int minY = Height - PaddingBottom - (int)((double)med.MinStockLevel / maxStock * chartHeight);
                    using var dashPen = new Pen(Color.Red, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                    g.DrawLine(dashPen, x, minY, x + BarWidth, minY);
                }

                // Draw stock quantity label above bar
                using var valueFont = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                string qtyLabel = med.StockQuantity.ToString();
                var labelSize = g.MeasureString(qtyLabel, valueFont);
                g.DrawString(qtyLabel, valueFont, Brushes.Black,
                    x + (BarWidth - labelSize.Width) / 2f,
                    Math.Max(barTop - labelSize.Height - 2, PaddingTop));

                // Draw medicine name below bar (truncated to 9 chars)
                using var nameFont = new Font("Segoe UI", 7F);
                string shortName = med.Name.Length > 9 ? med.Name[..9] + "\u2026" : med.Name;
                g.DrawString(shortName, nameFont, Brushes.DimGray,
                    x, Height - PaddingBottom + 4);
            }

            // Draw Y-axis baseline
            g.DrawLine(Pens.DimGray,
                PaddingLeft, PaddingTop,
                PaddingLeft, Height - PaddingBottom);

            // Draw X-axis baseline
            g.DrawLine(Pens.DimGray,
                PaddingLeft, Height - PaddingBottom,
                Width - 10, Height - PaddingBottom);
        }
    }
}
