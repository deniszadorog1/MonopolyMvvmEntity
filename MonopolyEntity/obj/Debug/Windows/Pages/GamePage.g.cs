﻿#pragma checksum "..\..\..\..\Windows\Pages\GamePage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F4A763B49A148E813C78EF2FC82E7DB85040310BF209E4CA242AED8932DEE3D0"
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
        
        
        #line 35 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard FirstPlayerRedControl;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard SecondPlayerBlueControl;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard ThirdPlayerGreenControl;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard FourthPlayerPurpleControl;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.UserCard FifthPlayerOrangeControl;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Windows\Pages\GamePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MonopolyEntity.Windows.UserControls.GameControls.GameField GameControl;
        
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
            this.FirstPlayerRedControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 2:
            this.SecondPlayerBlueControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 3:
            this.ThirdPlayerGreenControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 4:
            this.FourthPlayerPurpleControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 5:
            this.FifthPlayerOrangeControl = ((MonopolyEntity.Windows.UserControls.GameControls.UserCard)(target));
            return;
            case 6:
            this.GameControl = ((MonopolyEntity.Windows.UserControls.GameControls.GameField)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

