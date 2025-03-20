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
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Dynamic;

namespace MonopolyEntity.Windows.UserControls.GameControls.Other
{
    /// <summary>
    /// Логика взаимодействия для Dice.xaml
    /// </summary>
    public partial class Dice : UserControl
    {
        private int _cubeRes;
        private bool _ifFirst;
        public Dice(int cubeRes, bool isFirstCube)
        {
            _cubeRes = cubeRes;
            _ifFirst = isFirstCube;

            InitializeComponent();

            SetNeedRotationParams();
            CreateCube();

            SetHorizontalRotation();
            SetVerticalRotation();
        }

        private void CreateCube()
        {
            Viewport3D viewport = new Viewport3D();

            const int zPosition = 5;
            const int zLookDirection = -5;
            const int yUpDirection = 1;
            const int fieldOfView = 40;

            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, zPosition),
                LookDirection = new Vector3D(0, 0, zLookDirection),
                UpDirection = new Vector3D(0, yUpDirection, 0),
                FieldOfView = fieldOfView
            };

            const int vectorParam = -1;
            viewport.Camera = camera;
            var light = new DirectionalLight(Colors.White, new Vector3D(vectorParam, vectorParam, vectorParam));
            var lightVisual = new ModelVisual3D { Content = light };
            viewport.Children.Add(lightVisual);

            Model3DGroup diceModel = CreateDiceModel();
            var diceVisual = new ModelVisual3D { Content = diceModel };
            viewport.Children.Add(diceVisual);

            const int rotationParam = 1;
            _horizontalRotation = new AxisAngleRotation3D(new Vector3D(0, rotationParam, 0), 0);
            _verticalRotation = new AxisAngleRotation3D(new Vector3D(rotationParam, 0, 0), 0);

            _rotateTransforms.Children.Add(new RotateTransform3D(_horizontalRotation));
            _rotateTransforms.Children.Add(new RotateTransform3D(_verticalRotation));

            diceModel.Transform = _rotateTransforms;

            Content = viewport;
        }

        private Model3DGroup CreateDiceModel()
        {
            var modelGroup = new Model3DGroup();

            const int upTop = 1;
            const int downTop = -1;

            Point3D[] vertices =
            {
                new Point3D(downTop, downTop, upTop),  // 0
                new Point3D(upTop, downTop, upTop),   // 1
                new Point3D (upTop, upTop, upTop),    // 2
                new Point3D(downTop, upTop, upTop),   // 3
                new Point3D(downTop, downTop, downTop), // 4
                new Point3D(upTop, downTop, downTop),  // 5
                new Point3D(upTop, upTop, downTop),   // 6
                new Point3D(downTop, upTop, downTop)   // 7
            };

            int firstTop = 0; 
            int secondTop = 1; 
            int thirdTop = 2; 
            int fourthTop = 3; 
            int fifthTop = 4; 
            int sixthTop = 5; 
            int seventhTop = 6;
            int eightTop = 7; 
            
            int[][] faces =
            {
                new[] {firstTop, secondTop, thirdTop, fourthTop}, // Front
                new[] {secondTop, sixthTop, seventhTop, thirdTop}, // Right
                new[] {sixthTop, fifthTop, eightTop, seventhTop}, // Back
                new[] {fifthTop, firstTop, fourthTop, eightTop}, // Left
                new[] {fourthTop, thirdTop, seventhTop, eightTop}, // Top
                new[] {firstTop, fifthTop, sixthTop, secondTop}  // Bottom
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


            const int firstTopOfFaceIndex = 0;
            const int secondTopOfFaceIndex = 1;
            const int thirdTopOfFaceIndex = 2;
            const int fourthTopOfFaceIndex = 3;

            const int textureCordValue = 1;

            const int firstTriangleIndex = 0;
            const int secondTriangleIndex = 1;
            const int thirdTriangleIndex = 2;
            const int fourthTriangleIndex = 3;

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
                        vertices[face[firstTopOfFaceIndex]],
                        vertices[face[secondTopOfFaceIndex]],
                        vertices[face[thirdTopOfFaceIndex]],
                        vertices[face[fourthTopOfFaceIndex]]
                    }),
                    TriangleIndices = new Int32Collection(new[] { firstTriangleIndex, secondTriangleIndex, 
                        thirdTriangleIndex, thirdTriangleIndex, fourthTriangleIndex, firstTriangleIndex }),
                    TextureCoordinates = new PointCollection(new[]
                    {
                        new Point(0, textureCordValue),
                        new Point(textureCordValue, textureCordValue),
                        new Point(textureCordValue, 0),
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
        public DoubleAnimation _verticalAnimation; 

        private void SetHorizontalRotation()
        {
            const int animationDuration = 1;
            const int declarationDuration = 1; 
            _horizontalAnimation = new DoubleAnimation
            {
                From = 0,
                To = _horizontalTo,
                Duration = TimeSpan.FromSeconds(animationDuration),
                //AccelerationRatio = 0.3, 
                DecelerationRatio = declarationDuration,
                //RepeatBehavior = RepeatBehavior.Forever,

            };

            _horizontalAnimation.Completed += (sender, e) =>
            {
            };

            _horizontalRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, _horizontalAnimation);
        }

        private void SetVerticalRotation()
        {
            const double toSlowTime = 0.1;
            const double toFastTime = 0.9;
            _verticalAnimation = new DoubleAnimation
            {
                From = 0,
                To = _verticalTo,
                Duration = TimeSpan.FromSeconds(1),
                AccelerationRatio = toSlowTime,
                DecelerationRatio = toFastTime,
                //RepeatBehavior = RepeatBehavior.Forever,

            };
            _verticalRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, _verticalAnimation);
        }

        private int _horizontalTo;
        private int _verticalTo;

        private void SetNeedRotationParams()
        {
            (int hor, int vert) rotation = GetRotationParams(_cubeRes);

            _horizontalTo = rotation.hor;
            _verticalTo = rotation.vert;
        }

        private (int hor, int vert) GetRotationParams(int diceValue)
        {
            //(int horizontal, int vertical) res = (0, 0);

            return _ifFirst ? GetRightCubeRotValues(diceValue) : GetLeftCubeRotValues(diceValue)

;           //return res;
        }

        private (int, int) GetLeftCubeRotValues(int diceValue)
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

        private (int, int) GetRightCubeRotValues(int diceValue)
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
