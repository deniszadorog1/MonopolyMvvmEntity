   M S B A M L   `   `   `  ����I  EMonopolyEntity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:MonopolyEntity.Windows.UserControls.MainPage,MonopolyEntity.Windows.UserControls.MainPage  5      ?    9MonopolyEntity.Windows.UserControls.MainPage.UserAnimated   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Llocal:clr-namespace:MonopolyEntity.Windows.UserControls.MainPage     5      X TMaterialDesignThemes.MahApps, Version=0.0.11.0, Culture=neutral, PublicKeyToken=nullb ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5
      -   5           MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignFont#���  5      .��5      �� -   5        $��
ElemBorder��5       �� CornerRadius$ 5��6   
������ ._�5   
   �� .+�5      �� -   5        $��	AnimImage��5      $y�Visible=�5      $��40��5      $��40��6    $��5 5 0 0q�5       �� Left$	 150��5      
 �� Top$ 0��6&   5      � 5      -   5        $��UserMenu��5      $	��130��6%   $	��200��62   $y�Hidden=�5       K� ZIndex$	 100��6'   
������� � RowDefinitions 5       �� 5!      $��50 �6$   �� 5"      $��40 �6$   �� 5#      $��40 �6$   .+�5&      �� -   5        $��UpperRowUserName��5&   6    �� Orientation$ 
Horizontal=�6   .+�5'      '  !MaterialDesignThemes.Wpf.PackIcon  -   5        $��UserMenuIcon��5(      $y�Visible=�5'   .      Kind$ AccountCircle=�5(   3   $��45��5)      $��45��6+   
������ 5*      � 5,      $��10 5 0 0q�6    5-      �� 5.      �� 5/      .+�52      �� -   5        $��	LoginText��52   1   $��0��6$   
���   �$��There can be your login��6U   �� 53      -   5        $��	MoneyText��53   1   $��1��6$   
�������$��There can be your money��6U   54      55      � 56      $��1��6    57      �� 58      .+�5:      �� -	   5        $��SettingsBut��5:   *   $��0��6   
�������
�������$��2q�5=      $��0q�5>      $��Center=�5?      $
��Left=�5@      $��10 0 0 0q�5A      $	��198��5B      $��30��6#   $��14(�6/   $��Settings��5C       " MaterialDesignRaisedDarkButton#��[ 5E      	 � ToolTip$	 Open user settings��5F      {�5D      �� *DataContext.ControlsEnabled+
 �� RelativeSource
 �� *FindAncestor+ �� AncestorType$ WindowL�5I      5J      � 5L      $��2��6   .+�5M      �� -
   5        $��ExitBut��5M   *   $��2��6   
�������
�������$��2q�5P      $��0q�5Q      $��Center=�5R      $	��Top=�5S      $
��Left=�5T      $��10 0 0 0q�5U      $	��198��5V      $��30��6#   $��14(�6/   $��Exit ��5W      #��[ 5Y      $	 Exit from user account��5Z      {�5X      �� *DataContext.ControlsEnabled+
 �� *FindAncestor+$ WindowL�5]      5_      5b      5c      5e      5f      