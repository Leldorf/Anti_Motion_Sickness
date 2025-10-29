using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;

//using SharpDX.Direct;

namespace AntiMotionSickness
{
    public class AntiMotionSickness
    {
        private readonly GraphicsWindow _window;
        private readonly Graphics _graphics;

        private readonly Dictionary<string, SolidBrush> _brushes;
        private readonly Dictionary<string, Image> _images;

        private readonly string[] _lineTypesStr = { "center", "corner", "cross" };

        private LineConfig[] _lineConfigs;
        internal LineConfig[] LineConfigs => _lineConfigs;

        private bool _bUpdate = true;
        private float centerX, centerY;

        public AntiMotionSickness(int width = 1920, int height = 1080)
        {
            _brushes = new Dictionary<string, SolidBrush>();
            _images = new Dictionary<string, Image>();
            _lineConfigs = new LineConfig[3];


            _graphics = new Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true,
                //UseMultiThreadedFactories = false,
                //VSync = false,
                //WindowHandle = IntPtr.Zero
            };

            _window = new GraphicsWindow(_graphics)
            {
                IsTopmost = true,
                IsVisible = true,
                FPS = 12,
                X = 0,
                Y = 0,
                Width = width,
                Height = height
            };

            _window.DestroyGraphics += _window_DestroyGraphics;
            _window.DrawGraphics += _window_DrawGraphics;
            _window.SetupGraphics += _window_SetupGraphics;

            centerX = width / 2;
            centerY = height / 2;
        }

        ~AntiMotionSickness()
        {
            _window?.Dispose();
            _graphics?.Dispose();
        }



        internal void ConfigureLine(int lineType, LineConfig config)
        {
            _lineConfigs[lineType] = config;
            SetBrush(lineType);
            _bUpdate = true;
        }

        private void SetBrush(int lineType)
        {
            _brushes[_lineTypesStr[lineType]].Color = _lineConfigs[lineType].color;
            _brushes[_lineTypesStr[lineType] + "Border"].Color = _lineConfigs[lineType].borderColor;
        }

        public void SetColor(int lineType, Color color)
        {
            var config = _lineConfigs[lineType];
            config.color = color;
            _lineConfigs[lineType] = config;
        }

        private void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
        {
            var gfx = e.Graphics;

            for (int i = 0; i < 3; i++)
            {
                _brushes[_lineTypesStr[i]] = gfx.CreateSolidBrush(_lineConfigs[i].color);
                _brushes[_lineTypesStr[i] + "Border"] = gfx.CreateSolidBrush(_lineConfigs[i].borderColor);
            }

            _brushes["Transparent"] = gfx.CreateSolidBrush(0, 0, 0, 0);
            if (e.RecreateResources) return;
        }

        private void _window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
        {
            foreach (var pair in _brushes) pair.Value.Dispose();
            foreach (var pair in _images) pair.Value.Dispose();
        }

        private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            if(!_bUpdate) return;
            var gfx = e.Graphics;
            gfx.ClearScene(_brushes["Transparent"]);


            float distanceFromCenterX = gfx.Width / 3;
            float distanceFromCenterY = gfx.Height / 4;

            //center leticle
            if (_lineConfigs[0].isVisible)
            {
                var size = gfx.Height / 10f * _lineConfigs[0].size;
                var distance = gfx.Height / 10f * _lineConfigs[0].distance;
                for (int i = 0; i < 4; i++)
                {
                    double angle = i * Math.PI / 2;
                    float startX = centerX + (distance * (float)Math.Cos(angle));
                    float startY = centerY + (distance * (float)Math.Sin(angle));
                    float endX = startX + (size * (float)Math.Cos(angle));
                    float endY = startY + (size * (float)Math.Sin(angle));

                    if (_lineConfigs[0].hasBorder)
                    {
                        gfx.DrawLine(_brushes["centerBorder"], startX, startY, endX, endY, 
                            _lineConfigs[0].thickness + _lineConfigs[0].borderThickness);
                    }
                    gfx.DrawLine(_brushes["center"], startX, startY, endX, endY, _lineConfigs[0].thickness);
                }
            }
            float length = gfx.Height / 6;

            //corner leticles
            if (_lineConfigs[1].isVisible)
            {
                var size = gfx.Height / 6f * _lineConfigs[1].size;
                var distanceX = gfx.Width / 3f * _lineConfigs[1].distance;
                var distanceY = gfx.Height / 4f * _lineConfigs[1].distance;

                for (int i = 0; i < 4; i++)
                {
                    float angleX = i >= 2 ? 1f : -1f;
                    float angleY = i % 2 == 0 ? 1f : -1f;
                    float startX = centerX + (distanceX * angleX);
                    float startY = centerY + (distanceY * angleY);
                    float endX = startX + (size * angleX);
                    float endY = startY + (size * angleY);

                    if (_lineConfigs[1].hasBorder)
                    {
                        gfx.DrawLine(_brushes["cornerBorder"], startX, startY, startX, endY, 
                            _lineConfigs[1].thickness + _lineConfigs[1].borderThickness);
                        gfx.DrawLine(_brushes["cornerBorder"], startX, startY, endX, startY, 
                            _lineConfigs[1].thickness + _lineConfigs[1].borderThickness);
                    }
                    gfx.DrawLine(_brushes["corner"], startX, startY, startX, endY, _lineConfigs[1].thickness);
                    gfx.DrawLine(_brushes["corner"], startX, startY, endX, startY, _lineConfigs[1].thickness);
                }
            }

            //cross leticles
            if (_lineConfigs[2].isVisible)
            {
                var size = gfx.Height / 6f * _lineConfigs[2].size;
                var distanceX = gfx.Width / 3f * _lineConfigs[2].distance;
                var distanceY = gfx.Height / 4f * _lineConfigs[2].distance;

                for (int i = 0; i < 4; i++)
                {
                    double angle = i * Math.PI / 2;
                    float startX = centerX + (distanceX * (float)Math.Cos(angle));
                    float startY = centerY + (distanceY * (float)Math.Sin(angle));
                    float endX = startX + (size * (float)Math.Cos(angle));
                    float endY = startY + (size * (float)Math.Sin(angle));

                    if (_lineConfigs[2].hasBorder)
                    {
                        gfx.DrawLine(_brushes["crossBorder"], startX, startY, endX, endY, 
                            _lineConfigs[2].thickness + _lineConfigs[2].borderThickness);
                    }
                    gfx.DrawLine(_brushes["cross"], startX, startY, endX, endY, _lineConfigs[2].thickness);
                }
            }
            _bUpdate = false;
        }


        public void Run()
        {
            _window.Create();
        }

        public void Stop()
        {
            _window.Dispose();
            _graphics.Dispose();
        }
    }
}
