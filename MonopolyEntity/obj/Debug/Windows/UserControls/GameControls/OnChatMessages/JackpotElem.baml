   M S B A M L   `   `   `  ����I  EMonopolyEntity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Mclr-namespace:MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages?MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages  5      Q    KMonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.JackpotElem   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _localMclr-namespace:MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages     5      -   5        $	��250��5	      $	��Top=�6   .��5      �� -   5        $
��Elem��5   4   ���#E3E3E3  �� CornerRadius$  10��6"   ._�5   
   � $��5q�6    � RowDefinitions 5      �� 5      $��55 �6    �� 5      $��55 �6    �� 5      �� 5      $��65 �6    .+�5      � $��0��6   .+�5      �� -   5        $��CasinoPaceText��5   .   $��You are in casino!��5      $��22(�66   $��20 5 0 0q�5      6.   5      � 5      $��1��6   $��	20 0 20 0q�6    .+�5      �� -   5        $��DescribeText��5      $G��AChoose 1 - 3 numbers, make bid, if youll guess, get 1000k - 6000k��5      $��18(�5   1   $
��Wrap=�6?   5      �� 5      $��2��6    �� Orientation$ 
Horizontal=�6&   .+�5       � .+�5!      � $��	20 0 20 0q�6!   $��Center=�64   .+�5"      �� -   5        $��OneRibBorder��5"      
������ $��55��5#      $��55��6(   $  7��64   
���   �$��1q�5$   1   ._�5'      �� -   5        $��
OneRibGrid��5'   !   ��5(      �� *ActualWidth+ �� ElementName$ OneRibBorder����5)      �� *ActualHeight+$ OneRibBorder��5+      �� 5-      -   5        $��TwoRibBorder��5-      $��7 0 0 0q�61   
������ $��55��5/      $��55��6(   $  7��64   
���   �$��1q�50   1   ._�53      �� -   5        $��
TwoRibGrid��53   !   ��54      �� *ActualWidth+$ OneRibBorder����55      �� *ActualHeight+$ OneRibBorder��57      �� 59      -	   5        $��ThreeRibBorder��59      $��7 0 0 0q�63   
������ $��55��5;      $��55��6(   $  7��64   
���   �$��1q�5<   1   ._�5?      �� -
   5        $��ThreeRibGrid��5?   !   ��5@      �� *ActualWidth+$ OneRibBorder����5A      �� *ActualHeight+$ OneRibBorder��5C      �� 5E      -   5        $��FourRibBorder��5E      $��7 0 0 0q�62   
������ $��55��5G      $��55��6(   $  7��64   
���   �$��1q�5H   1   ._�5K      �� -   5        $��FourRibGrid��5K   !   ��5L      �� *ActualWidth+$ OneRibBorder����5M      �� *ActualHeight+$ OneRibBorder��5O      �� 5Q      -   5        $��FiveRibBorder��5Q      $��7 0 0 0q�62   
������ $��55��5S      $��55��6(   $  7��64   
���   �$��1q�5T   1   ._�5W      �� -   5        $��FiveRibGrid��5W   !   ��5X      �� *ActualWidth+$ OneRibBorder����5Y      �� *ActualHeight+$ OneRibBorder��5[      �� 5]      -   5        $��SixRibBorder��5]   !   $��7 0 0 0q�65   
������ $��55��5_      $��55��6(   $  7��64   
���   �$��1q�5`   1   ._�5c      �� -   5        $��
SixRibGrid��5c   %   ��5d      �� *ActualWidth+$ OneRibBorder����5e      �� *ActualHeight+$ OneRibBorder��5g      5h      5i      � 5k      $	��150��6   .+�5l      �� -   5        $��PotWon��5l       $��-��6.   $��28(�67   $��10 0 0 0q�6E   $��Center=�5m       $
��Left=�6;   5n      5o      � 5p      $��3��6    � ColumnDefinitions 5r      �� 5s      $��* �6'   �� 5t      $��3* �6'   �� 5u      -   5         �Name$ DontPlayColButDef��5u   2   $��3* �6'   62   �� 5v      $��* �6'   .+�5z      � -   5        $��PlayBuyGrid��5z   9   $��1��6   $��0 0 10 0q�6'   .+�5{      �� -   5        $��	LockImage��5{      $��40 0 0 0q�6.   $y�Visible=�6A   $��25��5|      $��25��6(   $
��Left=�64    K� ZIndex$	 100��6O   $6��0//Visuals/Images/BoardImages/Other/blackLock.png��5}      �� 5      -   5        $��
MakeBidBut��5      $��40��60      MainGlobalColor#��[  5�      
�������$��Center=�5�   -   $��Right=�5�      $��	Bid 1000k��65   
�������$��Center=�5�   ,   $��Center=�5�      $��14(�6=   ��5�      �� *ActualWidth+$ PlayBuyGrid��5�   =   5�      �� 5�      -   5        $��
DeclineBut��5�      $��2��6,   ���#ECEEF1$
��Left=�5�   .   
�������$��10 0 0 0q�5�   -   $��40��5�      $��Decline��5�      
���   �$��Center=�5�      $��Center=�5�      $��14(�6;   ��5�      �� *ActualWidth+$ PlayBuyGrid��5�   ;   5�      5�      5�      5�      