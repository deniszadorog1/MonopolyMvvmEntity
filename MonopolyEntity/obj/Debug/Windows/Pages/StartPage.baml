   M S B A M L   `   `   `  ����I  EMonopolyEntity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:MonopolyEntity.Windows.PagesMonopolyEntity.Windows.Pages  5      ,    &MonopolyEntity.Windows.Pages.StartPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:MonopolyEntity.Windows.Pages     5      U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  5      X TMaterialDesignThemes.MahApps, Version=0.0.11.0, Culture=neutral, PublicKeyToken=nullb ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5      $	��450��5	      $	��450��6   $	��450��5
      $	��450��6   $	��450��5      $	��450��6     N� Title$  	StartPage��5         MaterialDesignBody#���   5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5       N� 
Background
 �����.��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $��70 �6   �� 5      $��70 �6   �� 5      $��70 �6   �� 5      �� 5      .+�5"   
   � -   5        $��LeftPart��5"      $��0��6   .+�5$      �� $��26(�6   ���#5b5d67$��Authorization page ��5%       �� TextAlignment$
 Left=�64   $��10q�6I   50      �� 52   
   $��1��6   $C��=This is authorization page you can enter in your account here��6!   $��18(�53      $
��Wrap=�6"   ���#5b5d67$��	10 0 25 0q�6K   �� 55   
   $��2��6    �� Orientation$ Vertical=�6"   $��	10 0 30 0q�69   .+�57      �� $��Login��6   $��22(�6%   ���#5b5d6763   �� 58      -   5        $��LoginTextBox��59   ,   $��20(�58       � ToolTip$ Login��6%   ���#5b5d67  MainGlobalColor.  (MaterialDesignThemes.Wpf.TextFieldAssist   UnderlineBrush# [ 5;      	   DecorationVisibility$	 Hidden=�5<      
������ 
 �� 
CaretBrush

 �   �5A      5B      �� 5D   
   $��3��6   $ Vertical=�6"   $��	10 0 30 0q�69   .+�5F      �� $��Password��6   $��22(�6(   ���#5b5d6766   G� 5G      -   5        $��PasswordTextBox��5H   /   $��20(�5G      $ Password��6(   ���#5b5d67# [ 5J      $	 Hidden=�5K       G� 
CaretBrush
 �   �6W   5L      �� 5N   
   -   5         ��Name$	 Gif��5N      $��4��6   $��10 0 5 0q�6*   $	��135��6<   "  WpfAnimatedGif.ImageBehavior   RepeatBehavior$ Forever��5O      $
��Left=�69      AnimatedSource$ /Visuals/Gifs/dude.gif��5P      � 5S   
   -   5        $��	RightPart��5S      $��5��6   .+�5T      �� -   5        $��RightButtons��5T      $	��Top=�6.   $��10 0 0 0q�5U      $ 
Horizontal=�6,   .+�5V      �� -   5        $��LoginBut��5V      
�������#��[ 5X      $	��200��5Y      $
��Left=�6$   $��0 0 0 0q�6?   $��Log in��5Z      $��Center=�6)    " MaterialDesignRaisedDarkButton#��[ 5\      $ Login��5]        ThirdGlobalColor+  %MaterialDesignThemes.Wpf.RippleAssist   Feedback# [ 5^      {�5[      �� *DataContext.ControlsEnabled+ �� RelativeSource �� *FindAncestor+ �� AncestorType$ WindowL�5_      �� 5b      -   5        $��RegistrationBut��5c      
�������$��5 0 0 0q�5d      #��[ 5f      $��Registration��5g      $��Center=�6/   #��[ 5i      
�������$ Registration��5k      # [ 5l      ��5e      �� *ActualWidth+ �� ElementName$ LoginBut��{�5h      �� *DataContext.ControlsEnabled+ �� *FindAncestor+$ WindowL�5m      5n      5p      5q      5r      