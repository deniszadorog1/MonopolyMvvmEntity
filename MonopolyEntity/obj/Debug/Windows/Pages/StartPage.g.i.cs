﻿#pragma checksum "..\..\..\..\Windows\Pages\StartPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9B8E2629890D18F59A397EE653C58310A607ED4B4B122F5706257E77674042CB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.MahApps;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using MonopolyEntity.Windows.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfAnimatedGif;


namespace MonopolyEntity.Windows.Pages {
    
    
    /// <summary>
    /// StartPage
    /// </summary>
    public partial class StartPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\Windows\Pages\StartPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LeftPart;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\Windows\Pages\StartPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LoginTextBox;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\Windows\Pages\StartPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordTextBox;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\Windows\Pages\StartPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Gif;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\Windows\Pages\StartPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RightPart;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\Windows\Pages\StartPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel RightButtons;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\Windows\Pages\StartPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LoginBut;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\..\Windows\Pages\StartPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RegistrationBut;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MonopolyEntity;component/windows/pages/startpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\Pages\StartPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LeftPart = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.LoginTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 58 "..\..\..\..\Windows\Pages\StartPage.xaml"
            this.LoginTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.LoginBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 58 "..\..\..\..\Windows\Pages\StartPage.xaml"
            this.LoginTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.LoginBox_LostFocus);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PasswordTextBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 73 "..\..\..\..\Windows\Pages\StartPage.xaml"
            this.PasswordTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.PasswordBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 73 "..\..\..\..\Windows\Pages\StartPage.xaml"
            this.PasswordTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.PasswordBox_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Gif = ((System.Windows.Controls.Image)(target));
            return;
            case 5:
            this.RightPart = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.RightButtons = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.LoginBut = ((System.Windows.Controls.Button)(target));
            
            #line 95 "..\..\..\..\Windows\Pages\StartPage.xaml"
            this.LoginBut.Click += new System.Windows.RoutedEventHandler(this.LoginBut_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.RegistrationBut = ((System.Windows.Controls.Button)(target));
            
            #line 109 "..\..\..\..\Windows\Pages\StartPage.xaml"
            this.RegistrationBut.Click += new System.Windows.RoutedEventHandler(this.RegistrationBut_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

