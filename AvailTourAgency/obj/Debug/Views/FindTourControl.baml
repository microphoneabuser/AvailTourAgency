   M S B A M L   `   `   `  ����J  FAvailTourAgency, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AvailTourAgency.ViewsAvailTourAgency.Views  5      +    %AvailTourAgency.Views.FindTourControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  5      /local#clr-namespace:AvailTourAgency.Views  5      
�������.��5      � z�5   
   V� 5      .~�5      �� �� 5      �� 5      T� 5      5      .+�5   
   �� ���	#FF3580BF$��30(�6+   $
��Bold"�69   $��10q�6K   $��1��6W   �� 6e   $�Туры��6i   6x   �� 5   
   -   5          ��Name$  AddTour��5      $��
734,21,0,0q�6"      MaterialDesignFlatButton#��[  66   $��+ Добавить тур��6h   $
��Left=�5      $	��151��6,   ���	#FF0017FF$��32��6P   $	��Top=�6\    �� 	ClickMode$ Release=�5      6%   �� 5   
   $��Right=�6   $��50��61   $	��Top=�6=   $��30��6U   $��	0,12,34,0q�6`    �� Orientation$ 
Horizontal=�6s   .+�5      � $
��0,10q�6   .+�5      �� -   5        $  ButtonCloseFilter��5       0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 5      $��30��5      $��30��6$   $��0q�60   $	��Top=�5       $
��Left=�61   $��0q�6L   $y�	Collapsed=�5!   9   ��5   <   Z� ��6R   Z� .��5"      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Filter=�6.   ���#4CB1F7$��25��6Q   $��25��6]   5#      �� 5$      -   5        $  ButtonOpenFilter��5$      #��[ 5%      $��30��5&      $��30��6$   $��0q�60   $	��Top=�5'      $
��Left=�61   $��0q�6L   $y�Visible=�5(   8   ��5&   <   Z� ��6R   Z� .��5)        $ Filter=�6.   ���#4CB1F7$��25��6Q   $��25��6]   5*      5+      5,      � 5.   
   -   5         �Name$ 
FilterGrid��5.      $��
587,49,0,0q�6#    K� ZIndex$ 10��5/      $	��354��6!   $	��Top=�6.   $y�	Collapsed=�50      .+�51      ��  �� CornerRadius$ 10��6   $��2q�6'   ���	#FF4A88E2���	#FFE0F2FF$	��307��6j   $��Center=�6w   $��	0,12,0,35q�6�   ._�52      �� -   5         ��Name$ StackPanelMenu��52      $	��394��65   $��Center=�6A   $y�Visible=�6^   $	��284��6s   $	��Top=�6�   $��
10,14,10,0q�6�   .+�53      � .+�54      �� $��Номер:��6$   $��16(�62   $��SemiBold"�6@   ���	#FF3580BF$��Center=�55   ;   $��0,17,267.2,13.2q�56   $   6?   �� 57      -   5         ��Name$ IdFilterBox��57   "   $	��Top=�67   $��135,10,10.,0q�6O   $��14(�6e     MaterialDesignTextBox#��[ 58   "   ���	#FF3580BF.��59   R   5:      5;      � 5<      .+�5=      �� $��Название:��6$   $��16(�65   $��SemiBold"�6C   ���	#FF3580BF$��Center=�5>   ;   $��0,10,267.2,13.2q�5?   $   6?   �� 5@      -   5        $ NameFilterBox��5@   "   $	��Top=�69   $��135,10,10.,0q�6Q   $��14(�6g   #��[ 5A   "   ���	#FF3580BF.��6h   5B      5C      � 5E      .+�5F      �� $��Дата вылета:��6$   $��16(�68   $��SemiBold"�6F   ���	#FF3580BF$	��Top=�5G   ;   $��0,10,260.2,13.8q�5H   $   5I      (  "System.Windows.Controls.DatePicker  5K      -   5        $��FirstFlightDatePicker��5K   %    ( $MaterialDesignFloatingHintDatePicker#��[ 6B   $��	135,0,0,0q�5L   %   $��40��68   	 � Language$	 ru�6D   4  .System.Windows.Automation.AutomationProperties
   IsOffscreenBehavior$
 Onscreen=�5M   %   $
��Left=�5N   %   $	��110��6@   ���	#FF3580BF$	��Top=�5O   %   5P      �� 5Q      $��-��6$   $��25(�6-   $��SemiBold"�6;   ���	#FF3580BF$	��Top=�5R   ;   $��255,10,125,0q�5S   $   5T        5U      -	   5        $��SecondFlightDatePicker��5U   %   #��[ 6C   $��	275,0,0,0q�5V   %   $��40��68   $	 ru�6D   $
 Onscreen=�5W   %   $
��Left=�5X   %   $	��110��6@   ���	#FF3580BF$	��Top=�5Y   %   5Z      5[      � 5\      .+�5]      �� $��Стоимость:��6$   $��16(�66   $��SemiBold"�6D   ���	#FF3580BF$	��Top=�5^   ;   $��0,10,260.2,13.8q�5_   $   5`      �� 5b      -
   5        $��
FirstPrice��5b   "   #��[ 64   $��
135,10,0,0q�5c   %   $	 ru�69   $
 Onscreen=�5d   %   $
��Left=�5e   %   $	��110��6@   ���	#FF3580BF$	��Top=�5f   %   .��6l   5g      �� 5h      $��-��6$   $��25(�6-   $��SemiBold"�6;   ���	#FF3580BF$	��Top=�5i   ;   $��255,10,125,0q�5j   $   5k      �� 5l      -   5        $��SecondPrice��5l   "   #��[ 65   $��
275,10,0,0q�5m   %   $	 ru�69   $
 Onscreen=�5n   %   $
��Left=�5o   %   $	��110��6@   ���	#FF3580BF$	��Top=�5p   %   .��6m   5q      5r      � 5s      $��
0 10 10 10q�6   .+�5t      �� -   5         ��Name$ NotDeletedCheckBox��5t   #    d� 	IsChecked$
 TrueY�6?   $
��Left=�6P   $1��+Показывать неудаленные��6k   ���	#FF3580BF$��14(�6�   6�   �� 5u      -   5        $ DeletedCheckBox��5u   #   $��Right=�6<   $-��'Показывать удаленные��6X   ���	#FF3580BF$��14(�6�   6�   5v      � 5w      .+�5x      �� -   5        $  AppendFilterButton��5x   !   $ Release=�6=   $��0,10,215,10q�6l   $��Применить��6�    " MaterialDesignFlatDarkBgButton#��[ 6�   6�   �� 5y      -   5        $  CancelFilterButton��5y   !   $ Release=�6=   $��215,10,10,10q�6l   $��Отменить��6�    # MaterialDesignFlatLightBgButton#��[ 6�   ���	#FF3580BF6�   5z      5{      5}      5~      � 5�   
   -   5        $ 	GridPanel��5�      $��10,65,10,60q�6"   $y�Visible=�67   .+�5�      &   System.Windows.Controls.DataGrid  -   5          Name$ 	ToursGrid��5�         CanUserAddRows .     CanUserDeleteRows .     CanUserReorderColumns .     CanUserResizeRows .    CanUserResizeColumns .     ClipboardCopyMode$ IncludeHeader=�5�   +      
IsReadOnly .    AutoGenerateColumns .     GridLinesVisibility$	 All=�5�   G      HeadersVisibility$ Column=�6a    J� IsHitTestVisible . 
����������	#FF0554F2���	#4C0046FF   CanUserSortColumns .    SelectionMode$ Extended=�5�   +   ��5�   *   �� *
ToursItems+   Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      -   5        idColumn��,  &System.Windows.Controls.DataGridColumn   Header$ 
Номер��5�   R      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6a   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *ID+6a     5�      -   5        
nameColumn��$ Название��5�   V   $
 Auto 6h    6=   �� *Name+5�        5�      -   5        
cityColumn��$ 
Город��5�   V   $
 Auto 6e    6=   �� *City+5�      4	  .System.Windows.Controls.DataGridTemplateColumn	  5�       	  CellTemplate 5�      �� 5�      .R�5�   "   �� $��
ChooseTour��6�   -   5        #��[ 5�   )   $��0q�6b   $��2 1 2 2q�6m   $��20��6   $	��Top=�6�   ���	#FF3580BF$��Выбрать��5�   =   $ Release=�6O   6c   5�      5�      5�      5�      � 5�   
   $��
0,0,0,-0.4q�6   $��60��6#   $��Bottom=�6/   .+�5�      �� $ 
Horizontal=�6   $��0,0,0,10q�5�      $��Center=�5�      $��50��6:   $��Bottom=�6F   $	��290��6a   .+�5�      �� -   5        $  First��5�         MaterialDesignOutlinedButton#��[ 6(   $��4q�5�      $��<< К началу��5�      $��6q�5�      ���	#3390CAF9���	#FF3580BF$ Release=�5�   K   6u   �� 5�      -   5        $  Previous��5�      #��[ 6+   $��4q�5�      $��<��5�      $	��6,9q�5�      ���	#3390CAF9���	#FF3580BF$��35��5�   K   $ Release=�6V   6�   �� 5�      -   5        $
  Next��5�      #��[ 6'   $��4q�5�      $��>��5�      $	��6,9q�5�      ���	#FF3580BF���	#3390CAF9$��35��5�   K   $ Release=�6V   6   �� 5�      -   5        $
  Last��5�      #��[ 6'   $��4q�5�      $��>> К концу��5�      $��6q�5�      ���	#FF3580BF���	#3390CAF9$ Release=�5�   K   6t   5�      �� 5�      $ 
Horizontal=�6   $��	20,0,0,10q�5�      $
��Left=�5�      $��50��68   $��Bottom=�6D   $	��155��6_   .+�5�      � $��	0 10 5 10q�6   .+�5�      �� ���	#FF3580BF$��15(�67     MaterialDesignTextBlock#��[ 6E   $��Страница��6v   6�   5�      �� 5�      -   5          ��Name$  CurrentPageNum��5�        MaterialDesignComboBox#��[ 63   ! �� SelectedIndex$! 0��5�   #   $��Center=�65   ���	#FF3580BF���	#FF90CAF9���	#3390CAF9$��14(�5�   i   $��45��6w   5�      � 5�      $��	5 10 0 10q�6   .+�5�      �� ���	#FF3580BF$��15(�67   #��[ 6E   $
��из��6v   6�   5�      � 5�      $��	5 10 0 10q�6   .+�5�      �� -   5        " ��Name$" GlobalPageCount��5�       ���	#FF3580BF$��15(�6P   #��[ 6^   $��0��6�   6�   5�      5�      �� 5�      $ 
Horizontal=�6   $��	0,0,20,10q�5�      $��Right=�5�      $��50��69   $��Bottom=�6E   $	��238��6`   .+�5�      � $��
10 10 0 10q�6   .+�5�      �� ���	#FF3580BF$��15(�67   #��[ 6E   $��Показывать по��6v   6�   5�      � 5�      $��	5 10 0 10q�6   $��Center=�5�      .+�5�      �� -   5        $  NumberOfRecords��5�      #��[ 68   $��Center=�6h   ���	#FF3580BF���	#FF90CAF9���	#3390CAF9$��14(�5�   i   $��45��6w   5�      5�      � 5�      $��	5 10 0 10q�6   .+�5�      �� ���	#FF3580BF$��15(�67   #��[ 6E   $��записей��6v   6�   5�      5�      5�      �� 5�   
   -   5        $��
loadingGif��5�      "
  WpfAnimatedGif.ImageBehavior# 
  AnimatedSource$I# Cpack://application:,,,/AvailTourAgency;component/Assets/loading.gif��5�      $��Center=�5�      $��Center=�5�      $	��150��6/   $	��150��6;   5�      5�      