   M S B A M L   `   `   `  ����J  FAvailTourAgency, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AvailTourAgency.ViewsAvailTourAgency.Views  5      0    *AvailTourAgency.Views.FindHotelRoomControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      /local#clr-namespace:AvailTourAgency.Views  5      
�������.��5
      �� .+�5   
   �� $	��530��6   .+�5      � -   5          �Name$  	HomePanel��5      $��	0,0,0.2,0q�6&   $��61��69   $	��Top=�6E   .+�5      � .+�5      �� ���	#FF3580BF$��30(�67   $
��Bold"�6E   $��10q�6W   $��1��6c   �� 6q   $�Отель №��6u   6�   �� 5      -   5         ��Name$ 
IdComboBox��5         MaterialDesignComboBox#��[  63    �� SelectedIndex$ 0��5   !   $
��Left=�63   ���	#FF3580BF���	#FF90CAF9���	#3390CAF9$��22(�5   8   $	��150��6F   $��152,10,0,10q�5   !   $��41��66   $
��Left=�6B   $��Center=�5   !   {�.  5      5      5      � 5      $��0 10 0 0q�6   .+�5      �� ���	#FF3580BF �� CornerRadius$ 10��61   $��2q�6C   $	��455��6W   $	��Top=�6d   ._�5      � .+�5      �� $��Номера отеля:��6$   $��16(�69   $��SemiBold"�6G   ���	#FF3580BF$��
10,10,10,0q�6t    J� RenderTransformOrigin$ 
0.5,-9.667&�6�   $��34��6�   $	��Top=�6�   &   System.Windows.Controls.DataGrid  5      -   5          Name$ HotelRoomsGrid��5   #      CanUserAddRows .     CanUserDeleteRows .     CanUserReorderColumns .  	   CanUserResizeRows	 . 
   CanUserResizeColumns
 .     ClipboardCopyMode$ IncludeHeader=�5   -      
IsReadOnly .    AutoGenerateColumns .     GridLinesVisibility$	 All=�5   I      HeadersVisibility$ Column=�6c    J� IsHitTestVisible . 
����������	#FF0554F2���	#4C0046FF$��0,49,0,0q�5!   -   $	��396��6?   $	��Top=�6L   ��5   ;   �� *HotelRoomsItems+   Columns 5"      0  *System.Windows.Controls.DataGridTextColumn  5#   "   ,  &System.Windows.Controls.DataGridColumn   Header$ 
Номер��6L      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6[   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *ID+6[     5$   "   $ Первый тип��6Y   $
 Auto 6m    65   �� *FirstTypeOfRoom+6|     5%   "   $ Второй тип��6Z   $
 Auto 6n    65   �� *SecondTypeOfRoom+6}     5&   "   $ Количество��6R   $
 Auto 6f    65   �� *
Quantity+6u     5'   "   $ Цена за сутки��6Q   $
 Auto 6h    65   �� *	Price24+6w     5(   "   $ Описание��6U   $
 Auto 6g    65   �� *Description+6v   4  .System.Windows.Controls.DataGridTemplateColumn  5)   "      CellTemplate 5*   &   �� 5+   *   .R�5,   .   �� $��ChooseHotelRoom��6�   -   5         # MaterialDesignFlatLightBgButton#��[ 5,   5   $��0q�6n   $��2 1 2 2q�6y   $��20��6�   $	��Top=�6�   ���	#FF3580BF$��Выбрать��5-   =    �� 	ClickMode$ Release=�6O   6c   5.   +   50   #   53      54      55      56      57      58      59      