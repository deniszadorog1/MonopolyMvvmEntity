﻿#pragma checksum "..\..\..\..\..\..\..\Windows\UserControls\GameControls\OnChatMessages\TradeControls\AddMoneyElem.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9FC6B63D2A2FCD40A08C1AC0069D0BDB982CEA4D0E6B8DAE88A897F9B317B3E2"
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
using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.TradeControls;
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


namespace MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.TradeControls {
    
    
    /// <summary>
    /// AddMoneyElem
    /// </summary>
    public partial class AddMoneyElem : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\..\..\..\..\Windows\UserControls\GameControls\OnChatMessages\TradeControls\AddMoneyElem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Money;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\..\..\..\Windows\UserControls\GameControls\OnChatMessages\TradeControls\AddMoneyElem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ItemName;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\..\..\..\Windows\UserControls\GameControls\OnChatMessages\TradeControls\AddMoneyElem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AmountOfMoneyBox;
        
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
            System.Uri resourceLocater = new System.Uri("/MonopolyEntity;component/windows/usercontrols/gamecontrols/onchatmessages/tradec" +
                    "ontrols/addmoneyelem.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\..\Windows\UserControls\GameControls\OnChatMessages\TradeControls\AddMoneyElem.xaml"
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
            this.Money = ((System.Windows.Controls.Image)(target));
            return;
            case 2:
            this.ItemName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.AmountOfMoneyBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 38 "..\..\..\..\..\..\..\Windows\UserControls\GameControls\OnChatMessages\TradeControls\AddMoneyElem.xaml"
            this.AmountOfMoneyBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.AmountOfMoneyBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 39 "..\..\..\..\..\..\..\Windows\UserControls\GameControls\OnChatMessages\TradeControls\AddMoneyElem.xaml"
            this.AmountOfMoneyBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.AmountOfMoneyBox_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

