   M S B A M L   `   `   `  ����J  FAvailTourAgency, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AvailTourAgency.ViewsAvailTourAgency.Views  5      *    $AvailTourAgency.Views.FindTDPControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      /local#clr-namespace:AvailTourAgency.Views  5      
�������.��5
      �� .+�5   
   �� $	��530��6   .+�5      � -   5          �Name$  	HomePanel��5      $��	0,0,0.2,0q�6&   $��61��69   $	��Top=�6E   .+�5      � .+�5      �� ���	#FF3580BF$��30(�67   $
��Bold"�6E   $��10q�6W   $��1��6c   �� 6q   $�
Тур №��6u   6�   �� 5      -   5         ��Name$ 
IdComboBox��5         MaterialDesignComboBox#��[  63    �� SelectedIndex$ 0��5   !   $
��Left=�63   ���	#FF3580BF���	#FF90CAF9���	#3390CAF9$��22(�5   8   $	��150��6F   $��114,10,0,9.8q�5   !   $��41��67   $
��Left=�6C   $��Center=�5   !   {�.  5      �� 5      -   5         ��Name$ EditingIndicator��5       $y�	Collapsed=�6:   
�������$��24(�6c   $��Medium"�6q   $��10q�6�   $��Center=�6�   ��6�   Z� �� 6�   $-�'Режим редактирования��6�   6�   �� 5      -   5        $ comeBackButton��5       $��0 0 30 0q�68   $5��/< Вернуться к списку туров��6J   $��13(�6o   ���	#FF3580BF$��Center=�6�   $��Right=�6�   6�  5      5      � 5      $��0 10 0 0q�6   .+�5      �� ���	#FF3580BF �� CornerRadius$ 10��61   $��2q�6C   $	��455��6W   $	��Top=�6d   ._�5      � .+�5      �� $ ��Вариации тура:��6$   $��16(�6:   $��SemiBold"�6H   ���	#FF3580BF$��
10,10,10,0q�6u    J� RenderTransformOrigin$ 
0.5,-9.667&�6�   $��34��6�   $	��Top=�6�   &   System.Windows.Controls.DataGrid  5      -   5          Name$ TourDatesPricesGrid��5   #      CanUserAddRows .     CanUserDeleteRows .  	   CanUserReorderColumns	 .  
   CanUserResizeRows
 .    CanUserResizeColumns .     ClipboardCopyMode$ IncludeHeader=�5    -      
IsReadOnly .    AutoGenerateColumns .     GridLinesVisibility$	 All=�5!   I      HeadersVisibility$ Column=�6c    J� IsHitTestVisible . 
����������	#FF0554F2���	#4C0046FF$��0,49,0,0q�5#   -   $	��396��6?   $	��Top=�6L   ��5   @   �� *TourDatesPricesItems+   Columns 5$      0  *System.Windows.Controls.DataGridTextColumn  5%   "   ,  &System.Windows.Controls.DataGridColumn   Header$ 
Номер��6L      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6[      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *ID+6h   4  .System.Windows.Controls.DataGridTemplateColumn  5'   "   $ Дата вылета��69      SortMemberPath$ Дата вылета��6N      CanUserSort .    CellTemplate 5(   &   �� 5)   *   .R�5*   .   �� ��68   �� *FlyDateThere+5+   +      CellEditingTemplate 5-   &   �� 5.   *   .R�5/   .   (  "System.Windows.Controls.DatePicker  $��fDate��69      SelectedDateFormat$ Short=�6F    � Language$ ru�6a      SelectedDate 6o   �� *FlyDateThere+50   +   52   #     54   "   $* $Дата вылета обратно��69   $* $Дата вылета обратно��6V    .  55   &   �� 56   *   .R�57   .   �� ��68   �� *FlyDateBack+58   +    5:   &   �� 5;   *   .R�5<   .     $��sDate��69   $ Short=�6F   $ ru�6a    6o   �� *FlyDateBack+5=   +   5?   #     5A   "   $ Длительность��6�   $
 Auto 6�    65   �� *Length+ �� ValidatesOnExceptions .  �� StringFormat$	 {0}��6�     5B   "   $ Цена��6   $
 Auto 6�    65   �� *Price+ . $	 {0}��6�     5C   "   $ Авиакомпания��6Q   $
 Auto 6g    65   �� *	Airline+6v     5D   "   $ Самолет��6O   $
 Auto 6`    65   �� *Plane+6o     5E   "   $! Класс перелета��6U   $
 Auto 6m    65   �� *FlightClass+6|     5F   "   $ 
Багаж��6Q   $
 Auto 6`    65   �� *	Luggage+6o     5G   "   $ Питание��6O   $
 Auto 6`    65   �� *Meals+6o     5H   "   $ Количество��6R   $
 Auto 6f    65   �� *
Quantity+6u     5I   "    5J   &   �� 5K   *   .R�5L   .   �� $��	ChooseTDP��6�   -   5         # MaterialDesignFlatLightBgButton#��[ 5L   5   $��0q�6n   $��2 1 2 2q�6y   $��20��6�   $	��Top=�6�   ���	#FF3580BF$��Выбрать��5M   =     �� 	ClickMode$  Release=�6O   6c   5N   +   5P   #   5S      5T      5U      5V      5W      5X      5Y      