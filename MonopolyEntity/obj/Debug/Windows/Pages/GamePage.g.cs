﻿#pragma checksum "..\..\..\..\Windows\Pages\GamePage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "176355B00EF5CB3C7D8D7465C32C6E02D89670697EF9E3AD7B95CEB33C0D2F8A"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MonopolyEntity.Windows.Pages;
using MonopolyEntity.Windows.UserControls.GameControls;
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


namespace MonopolyEntity.Windows.Pages {
    
    
    /// <summary>
    /// GamePage
    /// </summary>
    public partial class GamePage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid UserCards;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard FirstPlayerRedControl;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard SecondPlayerBlueControl;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard ThirdPlayerGreenControl;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard FourthPlayerPurpleControl;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard FifthPlayerOrangeControl;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid FieldGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/MonopolyEntity;component/windows/pages/gamepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\Pages\GamePage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 14 "..\..\..\..\Windows\Pages\GamePage.xaml"
            ((MonopolyEntity.Windows.Pages.GamePage)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Page_PreviewMouseDown);
            
            #line default
            #line hidden
            
            #line 15 "..\..\..\..\Windows\Pages\GamePage.xaml"
            ((MonopolyEntity.Windows.Pages.GamePage)(target)).SizeChanged += new System.Windows.SizeChangedEventHandler(this.Page_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.UserCards = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.FirstPlayerRedControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 4:
            this.SecondPlayerBlueControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 5:
            this.ThirdPlayerGreenControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 6:
            this.FourthPlayerPurpleControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 7:
            this.FifthPlayerOrangeControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 8:
            this.FieldGrid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

