   M S B A M L   `   `   `  ����I  EMonopolyEntity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Mclr-namespace:MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages?MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages  5      Q    KMonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.JackpotElem   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _localMclr-namespace:MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages     5      -   5        $	��575��5   7   $	��250��5	      $	��Top=�6   .��5      �� ���#E3E3E3  �� CornerRadius$  10��6"   ._�5   
   � $��5q�6    � RowDefinitions 5      �� 5      $��55 �6    �� 5      $��55 �6    �� 5      �� 5      $��65 �6    .+�5      � $��0��6   .+�5      �� -   5        $��CasinoPaceText��5   .   $��You are in casino!��5      $��22(�66   $��20 5 0 0q�5      6.   5      � 5      $��1��6   $��	20 0 20 0q�6    .+�5      �� -   5        $��DescribeText��5      $G��AChoose 1 - 3 numbers, make bid, if youll guess, get 1000k - 6000k��5      $��18(�5   1   5      � 5      $��2��6   .+�5       � $��	20 0 20 0q�6   $��Center=�60   .+�5!      �� -   5        $��OneRibBorder��5!      
������ $��55��5"      $��55��6(   $  7��64   
���   �$��1q�5#   1   ._�5&      �� -   5        $��
OneRibGrid��5&   !   ��5'      �� *ActualWidth+ �� ElementName$ OneRibBorder����5(      �� *ActualHeight+$ OneRibBorder��5*      �� 5,      -   5        $��TwoRibBorder��5,      $��7 0 0 0q�61   
������ $��55��5.      $��55��6(   $  7��64   
���   �$��1q�5/   1   ._�52      �� -   5        $��
TwoRibGrid��52   !   ��53      �� *ActualWidth+$ OneRibBorder����54      �� *ActualHeight+$ OneRibBorder��56      �� 58      -   5        $��ThreeRibBorder��58      $��7 0 0 0q�63   
������ $��55��5:      $��55��6(   $  7��64   
���   �$��1q�5;   1   ._�5>      �� -	   5        $��ThreeRibGrid��5>   !   ��5?      �� *ActualWidth+$ OneRibBorder����5@      �� *ActualHeight+$ OneRibBorder��5B      �� 5D      -
   5        $��FourRibBorder��5D      $��7 0 0 0q�62   
������ $��55��5F      $��55��6(   $  7��64   
���   �$��1q�5G   1   ._�5J      �� -   5        $��FourRibGrid��5J   !   ��5K      �� *ActualWidth+$ OneRibBorder����5L      �� *ActualHeight+$ OneRibBorder��5N      �� 5P      -   5        $��FiveRibBorder��5P      $��7 0 0 0q�62   
������ $��55��5R      $��55��6(   $  7��64   
���   �$��1q�5S   1   ._�5V      �� -   5        $��FiveRibGrid��5V   !   ��5W      �� *ActualWidth+$ OneRibBorder����5X      �� *ActualHeight+$ OneRibBorder��5Z      �� 5\      -   5        $��SixRibBorder��5\      $��7 0 0 0q�61   
������ $��55��5^      $��55��6(   $  7��64   
���   �$��1q�5_   1   ._�5b      �� -   5        $��
SixRibGrid��5b   !   ��5c      �� *ActualWidth+$ OneRibBorder����5d      �� *ActualHeight+$ OneRibBorder��5f      5h      5i      � 5k      $��3��6   .+�5m      ��  �� Orientation$ 
Horizontal=�6   $��Center=�66   .+�5o      � .+�5p      �� -   5        $��	LockImage��5p      $��45 0 0 0q�60   $y�Visible=�6C   $��25��5q      $��25��6(   $
��Left=�64    K� ZIndex$	 100��6O   $6��0//Visuals/Images/BoardImages/Other/blackLock.png��5r      �� 5t      -   5        $��
MakeBidBut��5t   !   $	��200��64   $��40��6@      MainGlobalColor#��[  5u      
�������$��Center=�5v   -   $��Center=�5w      $��	Bid 1000k��66   
�������$��Center=�5x   ,   $��Center=�5y      $��14(�6=   5z      �� 5{      -   5        $��
DeclineBut��5{      ���#ECEEF1
�������$��10 0 0 0q�5|   )   $��Decline��5      
���   �$��Center=�5�      $��Center=�5�      $��14(�67   ��5}      �� *ActualWidth+$ 
MakeBidBut����5~      �� *ActualHeight+$ 
MakeBidBut��5�   7   5�      5�      5�      5�      5�      