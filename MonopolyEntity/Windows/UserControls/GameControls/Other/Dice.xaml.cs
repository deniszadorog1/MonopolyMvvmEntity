using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;

using MonopolyEntity.VisualHelper;
using System.Windows.Media.Animation;
using System.Windows.Markup.Localizer;
using System.Threading;

namespace MonopolyEntity.Windows.UserControls.GameControls.Other
{
    /// <summary>
    /// Логика взаимодействия для Dice.xaml
    /// </summary>
    public partial class Dice : UserControl
    {
        private int _cubeRes;
        private bool _ifFirst;
        public Dice(int cubeRes, bool ifFirstCube)
        {
            _cubeRes = cubeRes;
            _ifFirst = ifFirstCube;

            InitializeComponent();

            SetNeedRoatationParams();
            CreateCube();

            SetHorizontalRotation();
            SetVerticalRoation();
        }

        private void CreateCube()
        {
            Viewport3D viewport = new Viewport3D();

            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 5),
                LookDirection = new Vector3D(0, 0, -5),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 40
            };

            viewport.Camera = camera;

            var light = new DirectionalLight(Colors.White, new Vector3D(-1, -1, -1));
            var lightVisual = new ModelVisual3D { Content = light };
            viewport.Children.Add(lightVisual);

            Model3DGroup diceModel = CreateDiceModel();
            var diceVisual = new ModelVisual3D { Content = diceModel };
            viewport.Children.Add(diceVisual);

            _horizontalRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            _verticalRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);

            _rotateTransforms.Children.Add(new RotateTransform3D(_horizontalRotation));
            _rotateTransforms.Children.Add(new RotateTransform3D(_verticalRotation));

            diceModel.Transform = _rotateTransforms;

            Content = viewport;
        }

        private Model3DGroup CreateDiceModel()
        {
            var modelGroup = new Model3DGroup();

            Point3D[] vertices =
            {
                new Point3D(-1, -1, 1),  // 0
                new Point3D(1, -1, 1),   // 1
                new Point3D (1, 1, 1),    // 2
                new Point3D(-1, 1, 1),   // 3
                new Point3D(-1, -1, -1), // 4
                new Point3D(1, -1, -1),  // 5
                new Point3D(1, 1, -1),   // 6
                new Point3D(-1, 1, -1)   // 7
            };

            int[][] faces =
            {
                new[] {0, 1, 2, 3}, // Front
                new[] {1, 5, 6, 2}, // Right
                new[] {5, 4, 7, 6}, // Back
                new[] {4, 0, 3, 7}, // Left
                new[] {3, 2, 6, 7}, // Top
                new[] {0, 4, 5, 1}  // Bottom
            };

            string diceFolderPath = BoardHelper.GetDiceFolderPath();

            string[] texturePaths =
            {
                System.IO.Path.Combine(diceFolderPath, "one.png"),
                System.IO.Path.Combine(diceFolderPath, "two.png"),
                System.IO.Path.Combine(diceFolderPath, "six.png"),
                System.IO.Path.Combine(diceFolderPath, "five.png"),
                System.IO.Path.Combine(diceFolderPath, "three.png"),
                System.IO.Path.Combine(diceFolderPath, "four.png"),
            };

            foreach (var face in faces)
            {
                var material = new DiffuseMaterial
                {
                    Brush = new ImageBrush(new BitmapImage(
                        new Uri(texturePaths[Array.IndexOf(faces, face)], UriKind.Relative)))
                };

                var mesh = new MeshGeometry3D
                {
                    Positions = new Point3DCollection(new[]
                    {
                        vertices[face[0]],
                        vertices[face[1]],
                        vertices[face[2]],
                        vertices[face[3]]
                    }),
                    TriangleIndices = new Int32Collection(new[] { 0, 1, 2, 2, 3, 0 }),
                    TextureCoordinates = new PointCollection(new[]
                    {
                        new Point(0, 1),
                        new Point(1, 1),
                        new Point(1, 0),
                        new Point(0, 0)
                    })
                };

                modelGroup.Children.Add(new GeometryModel3D(mesh, material));
            }

            return modelGroup;
        }

        private Transform3DGroup _rotateTransforms = new Transform3DGroup();

        private AxisAngleRotation3D _horizontalRotation;
        private AxisAngleRotation3D _verticalRotation;

        public DoubleAnimation _horizontalAnimation;
        public DoubleAnimation _vertialAnimation;

        private void SetHorizontalRotation()
        {
            //return;
            _horizontalAnimation = new DoubleAnimation
            {
                From = 0,
                To = _horizontalTo,
                Duration = TimeSpan.FromSeconds(1),
                //AccelerationRatio = 0.3, 
                DecelerationRatio = 1,
                //RepeatBehavior = RepeatBehavior.Forever,

            };

            _horizontalAnimation.Completed += (sender, e) =>
            {
            };

            _horizontalRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, _horizontalAnimation);
        }

        private void SetVerticalRoation()
        {
            _vertialAnimation = new DoubleAnimation
            {
                From = 0,
                To = _verticalTo,
                Duration = TimeSpan.FromSeconds(1),
                AccelerationRatio = 0.1,
                DecelerationRatio = 0.9,
                //RepeatBehavior = RepeatBehavior.Forever,

            };
            _verticalRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, _vertialAnimation);
        }

        private int _horizontalTo;
        private int _verticalTo;

        private void SetNeedRoatationParams()
        {
            (int hor, int vert) rotation = GetRoationParams(_cubeRes);

            _horizontalTo = rotation.hor;
            _verticalTo = rotation.vert;
        }

        private (int hor, int vert) GetRoationParams(int diceValue)
        {
            //(int horizontal, int vertical) res = (0, 0);

            return _ifFirst ? GetRightCubeRotVals(diceValue) : GetLeftCubeRotVals(diceValue)

;           //return res;
        }

        private (int, int) GetLeftCubeRotVals(int diceValue)
        {
            (int horizontal, int vertical) res = (0, 0);
            const int baseValue = -1080;
            const int oneVertical = -1800;
            const int twoHorizontal = -2250;
            const int threeVertical = -2070;
            const int fourVertical = -2250;
            const int fiveHorizontal = -2070;
            const int sixVertical = -1980;

            switch (diceValue)
            {
                case 1:
                    {
                        res.vertical = oneVertical;
                        res.horizontal = baseValue;
                        break;
                    }
                case 2:
                    {
                        res.vertical = baseValue;
                        res.horizontal = twoHorizontal;
                        break;
                    }
                case 3:
                    {
                        res.vertical = threeVertical;
                        res.horizontal = baseValue;
                        break;
                    }
                case 4:
                    {
                        res.vertical = fourVertical;
                        res.horizontal = baseValue;
                        break;
                    }
                case 5:
                    {
                        res.vertical = baseValue;
                        res.horizontal = fiveHorizontal;
                        break;
                    }
                case 6:
                    {
                        res.vertical = sixVertical;
                        res.horizontal = baseValue;
                        break;
                    }
            }
            return res;
        }

        private (int, int) GetRightCubeRotVals(int diceValue)
        {
            (int horizontal, int vertical) res = (0, 0);

            const int baseValue = 1080;

            const int oneVertical = 1800;
            const int twoHorizontal = 2070;
            const int threeVertical = 1890;
            const int fourVertical = 2070;
            const int fiveHorizontal = 1890;
            const int sixVertical = 1980;

            switch (diceValue)
            {
                case 1:
                    {
                        res.vertical = oneVertical;
                        res.horizontal = baseValue;
                        break;
                    }
                case 2:
                    {
                        res.vertical = baseValue;
                        res.horizontal = twoHorizontal;
                        break;
                    }
                case 3:
                    {
                        res.vertical = threeVertical;
                        res.horizontal = baseValue;
                        break;
                    }
                case 4:
                    {
                        res.vertical = fourVertical;
                        res.horizontal = baseValue;
                        break;
                    }
                case 5:
                    {
                        res.vertical = baseValue;
                        res.horizontal = fiveHorizontal;
                        break;
                    }
                case 6:
                    {
                        res.vertical = sixVertical;
                        res.horizontal = baseValue;
                        break;
                    }
            }

            return res;
        }
    }
}
