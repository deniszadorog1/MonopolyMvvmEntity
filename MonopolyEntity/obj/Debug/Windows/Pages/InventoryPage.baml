   M S B A M L   `   `   `  ����I  EMonopolyEntity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:MonopolyEntity.Windows.PagesMonopolyEntity.Windows.Pages  5      Y1clr-namespace:MonopolyEntity.Windows.UserControls#MonopolyEntity.Windows.UserControls  5      0    *MonopolyEntity.Windows.Pages.InventoryPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:MonopolyEntity.Windows.Pages     5      KAddedControls1clr-namespace:MonopolyEntity.Windows.UserControls     5      X TMaterialDesignThemes.MahApps, Version=0.0.11.0, Culture=neutral, PublicKeyToken=nullb ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5      -   5          N� Title$  InventoryPage��5	         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignFont N� 
FontFamily# �  5      .��5   
   � -   5        $��MyGrid��5   $   ���#F4F4F5 � RowDefinitions 5      �� 5      $��50 �6    �� 5      $	��600 �6    �� 5       � ColumnDefinitions 5       �� 5!      �� 5"      -   5         �Name$ CenterColDef��5"   0   $
��1000 �6#   60   �� 5#      .+�5&      � -   5        $��	UpperPart��5&   0   $��0��6   $��1��6    $
��Left=�5'      
������� K� ZIndex$	 100��5(   "   $
��1000��66   .+�5)      3   -MonopolyEntity.Windows.UserControls.UpperMenu  -   5          Name$ 
UpperMenuu��5)   *   $
��1000��6>   5*      � 5,      $��0��6   $��0��6    
�������60   � 5-      $��0��6   $��2��6    
�������60   �� 5/      		 �� X1$	 0��6   	
 �� Y1$
 1��6   $��1��6"   $��0��6/   	 �� Y2$ 1��51      
�������$��1��6)   	 �� X2 50      �� *ActualWidth+ �� ElementName$ AllGrid��51   )   �� 53      $	 0��6   $
 1��6   $��1��6"   $��1��6/   $ 1��55      
�������$��1��6)    54      �� *ActualWidth+$ AllGrid��55   )   �� 57      $	 0��6   $
 1��6   $��1��6"   $��2��6/   $ 1��59      
�������$��1��6)    58      �� *ActualWidth+$ AllGrid��59   )   � 5;      $��1��6   $��1��6     5=      �� 5>      $	��100 �6$   �� 5?      $��50 �6$   �� 5@      .+�5C      � $��0��6   .+�5D      �� -   5        $��	UserImage��5D      $��10q�6.   $��75��6:   $��75��6E   �� 5F      -   5        $��	UserLogin��5F      $��There sdhould be login��60   $��Center=�6Q   $��38(�5G      
�������$��75��6<   
������ $��0q�6a   �� 5I      -   5        $��UpperInventoryDevider��5I       $��> ��6>   $��Center=�6H   $��36(�5J      
�������6*   �� 5L      -	   5        $��InventoryStatement��5L       $��Inventory  ��6;   $��Center=�6N   
�������$��36(�5M   .   �� 5O      -
   5        $��AmountOfItems��5O       $	��999��65   $��Center=�6@   
�������$��36(�5P   .   5Q      � 5S      $��1��6   .+�5U      �� 
�������$��1q�60   $��	10 10 0 0q�6D    �� CornerRadius$ 5��5V      $��35��6*   $	��450��66   ._�5W      �� -   5        $��FilterByBameBox��5Y      $��16(�5W   "    �� 	MaxLength$ 32��5X      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Enter Text��5[      .  (MaterialDesignThemes.Wpf.TextFieldAssist   UnderlineBrush
 ���� 5\      5]      �� 5`      $��40��6   $	��200��6)   $��	20 10 0 0q�65   $ 5��6H   
�������$��1q�5a   ,   ._�5b      �� -   5        $��TypeChooseBox��5c   B   $	��Top=�5b   #   
������ $��40��5c   6   $ 	All Items��5e      $��Center=�5f      .@�5g      �� $��Items��6+   �� 5h      $	��Box��6+   �� 5i      -   5        $��BaseChooseType��5i   9   $	��All��6+   69   5j      5k      �� 5m      $��40��6   $	��200��6)   $��	20 10 0 0q�65   $ 5��6H   
�������$��1q�5n   ,   ._�5o      �� -   5        $��
RareFilter��5p   #   $	��Top=�5o   #   
������ $��40��5q   .   $ All Rearities��5r      $��Center=�5s      .@�5t      �� $��Usual��6+   �� 5u      $
��Rare��6+   �� 5v      $��VeryRare��6+   �� 5w      $��ExtrimlyRare��6+   �� 5x      $��Legend��6+   �� 5y      -   5        $��BaseChooseRare��5y   9   $	��All��6+   69   5z      5{      5}      �� 5      $��2��6    $
��Auto=�6-   $	��900��5�       $
��Left=�6,   $��0 10 0 0q�6G   .��5�      � -   5        $��
ItemsPanel��5�       
������ $	��850��5�   9   $��10q�6E   $
��Left=�5�   2   5�      5�      5�      5�      5�      