   M S B A M L   `   `   `  ����J  FAvailTourAgency, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AvailTourAgency.ViewsAvailTourAgency.Views  5      *    $AvailTourAgency.Views.AddServiceView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      /local#clr-namespace:AvailTourAgency.Views  5      
�������.��5
      � .+�5   
   �� -   5          ��Name$  	HomePanel��5      .+�5      � .+�5      �� ���	#FF3580BF$��30(�63   $
��Bold"�6A   $��10q�6S   $��1��6_   �� 6m   $�Услуга №��6q   6�   �� 5      -   5         ��Name$ IdBox��5       �� 
IsReadOnly . $
��Left=�5   #   ���	#FF3580BF���	#FF90CAF9���	#3390CAF9$��22(�5   :   $	��150��6H   $��159,10,0,9.8q�5   #   $��41��69   $��Center=�6E   $��Center=�5   #   .��6E   5      �� 5      -   5         ��Name$ EditingIndicator��5      $y�	Collapsed=�66   
�������$��24(�6_   $��Medium"�6m   $��10q�6�   $��Center=�6�   ��6�   Z� �� 6�   $-�'Режим редактирования��6�   6�   �� 5      -   5        $ comeBackButton��5      $��0 0 30 0q�64   $5��/< Вернуться к списку услуг��6F   $��13(�6k   ���	#FF3580BF$��Center=�6�   $��Right=�6�   6�  5      5      �� 5   
   -   5        $  CurrentAddServicePanel��5      $��10,50,10,47q�65   $y�Visible=�6J   .+�5      � $��0 0 0 0q�6   $	��432��6$   .+�5      � -   5         �Name$ ViewAddServiceGrid��5      $y�Visible=�63   $	��172��6H   $	��Top=�6U   .+�5      �� $
��Left=�6!   $	��480��6<   $	��157��6H   $	��Top=�6U   $��	0,0,0,-54q�6m   .+�5      � .+�5      �� $��Название:��6(   $��16(�69   $��SemiBold"�6G   ���	#FF3580BF$��5 20 0 5q�6t   $��Center=�6�   6�   �� 5      -   5        $��NameBox��5   &      MaterialDesignTextBox#��[  65    � Language$ ru�5    )    . $��14(�6I   4  .System.Windows.Automation.AutomationProperties   IsOffscreenBehavior$ Onscreen=�5!   )   $
��Left=�5"   )   $	��360��6D   ���	#FF3580BF$	��Top=�5#   )   $��
110,15,0,0q�6A   $��31��6U   .��6a   5$      5%      � 5&      .+�5'      �� $��	Цена:��6(   $��16(�65   $��SemiBold"�6C   ���	#FF3580BF$��5 20 0 5q�6p   $��Center=�6�   6�   �� 5(      -   5        $��PriceBox��5(   &   #��[  66   $ ru�5)   )    . $��14(�6I   $ Onscreen=�5*   )   $
��Left=�5+   )   $	��360��6D   ���	#FF3580BF$	��Top=�5,   )   $��
110,15,0,0q�6A   $��33��6U   .��6a   5-      5.      � 5/      .+�50      �� $��Тур:��6(   $��16(�64   $��SemiBold"�6B   ���	#FF3580BF$��5 20 0 5q�6o   $��Center=�6�   6�   �� 51      -	   5        $��TourBox��51   &   #��[  65   $ ru�52   )    . $��14(�6I   $ Onscreen=�53   )   $
��Left=�54   )   $	��360��6D   ���	#FF3580BF$	��Top=�55   )   $��
110,15,0,0q�6A   $��33��6U   .��6a   56      57      58      �� 59      $��	502,0,0,0q�6!   $	��157��64   $	��Top=�6A   $
��Left=�6Y   $	��492��6t   .+�5:      � $	��157��6   .+�5;      �� $��Описание:��6(   $��16(�69   $��SemiBold"�6G   ���	#FF3580BF$��5,20,0,0q�6t   $	��Top=�6�   6�   �� 5<      -
   5        $��DescriptionBox��5<   &   #��[  6<   $ ru�5=   -   $��14(�6;    �� TextWrapping$
 Wrap=�6I    �� AcceptsReturn .  . $ Onscreen=�5>   -   $
��Left=�5?   -   $	��360��6H   ���	#FF3580BF$	��Top=�5@   -   $��	110,0,0,0q�6E   $	��147��6X   $	��0.5q�6e   .��6{   5A      5B      5C      5D      � 5E      -   5        $ EditAddServiceGrid��5E      $y�	Collapsed=�63   $��	0,0,0.2,0q�6J   $	��103��6]   $	��Top=�6j   .+�5F      �� $	��202��6!   $	��Top=�6.   $��	0,0,0,-99q�6F   $
��Left=�6Y   $	��480��6t   .+�5G      � .+�5H      �� $��Название:��6(   $��16(�69   $��SemiBold"�6G   ���	#FF3580BF$��5 20 0 5q�6t   $��Center=�6�   6�   �� 5I      -   5        $��EditNameBox��5I   &   #��[  69   $ ru�5J   )    .  $��14(�6J   $ Onscreen=�5K   )   $
��Left=�5L   )   $	��360��6D   ���	#FF3580BF$	��Top=�5M   )   $��
110,15,0,0q�6A   $��31��6U   .��6�   5N      5O      � 5P      .+�5Q      �� $��	Цена:��6(   $��16(�65   $��SemiBold"�6C   ���	#FF3580BF$��5 20 0 5q�6p   $��Center=�6�   6�   �� 5R      -   5        $��EditPriceBox��5R   &   #��[  6:   $ ru�5S   )    .  $��14(�6J   $ Onscreen=�5T   )   $
��Left=�5U   )   $	��360��6D   ���	#FF3580BF$	��Top=�5V   )   $��
110,15,0,0q�6A   $��33��6U   .��6�   5W      5X      � 5Y      .+�5Z      �� $��Тур:��6(   $��16(�64   $��SemiBold"�6B   ���	#FF3580BF$��5 20 0 5q�6o   $��Center=�6�   6�   �� 5[      -   5        $��EditTourBox��5[   &   #��[  69   $ ru�5\   )    . $��14(�6I   $ Onscreen=�5]   )   $
��Left=�5^   )   $	��260��6D   ���	#FF3580BF$	��Top=�5_   )   $��
110,15,0,0q�6A   $��31��6U   .��6a   5`      �� 5a      -   5        	 ��Name$	 FindTour��5a   %   $��	0,15,10,0q�67     MaterialDesignFlatButton#��[ 6J   $��
Найти��6|   $��Right=�6�   $��95��6�   ���	#FF0017FF$��31��6�   
 �� 	ClickMode$
 Release=�6�   6�   5b      �� 5c      -   5        $	 	ClearTour��5c   !   $��	0,15,10,0q�64   #��[ 6G   $��Очистить��6y   $��Right=�6�   $��95��6�   ���	#FF0017FF$��31��6�   $
 Release=�6�   6�   5d      �� 5e      $��502,0,0,-54q�6!   $	��157��66   $	��Top=�6C   $
��Left=�6[   $	��492��6v   .+�5f      � $	��157��6   .+�5g      �� $��Описание:��6(   $��16(�69   $��SemiBold"�6G   ���	#FF3580BF$��5,20,0,0q�6t   $	��Top=�6�   6�   �� 5h      -   5        $��EditDescriptionBox��5h   &   #��[  6@   $ ru�5i   -   $��14(�6;   $
 Wrap=�6I    .  .  $ Onscreen=�5j   -   $
��Left=�5k   -   $	��360��6H   ���	#FF3580BF$	��Top=�5l   -   $��	110,0,0,0q�6E   $	��147��6X   $	��0.5q�6e   .��6{   5m      5n      5o      5p      5q      5r      � 5s   
   $��Center=�6   $
��1024��6,   .+�5t      �� -   5        $	 PrevBtn��5t      $��	50,0,0,10q�6&    / +MaterialDesignFloatingActionMiniLightButton#��[ 69   $��<��6~   $
��Left=�6�   $��32��6�   $��32��6�   $��Bottom=�6�   $
 Release=�6�   6  �� 5u      -   5        $	 EditAddService��5u      $��
200,0,0,10q�6-   #��[ 6A   $��Изменить��6s   $
��Left=�6�   $	��150��6�   ���	#FF0017FF$��32��6�   $��Bottom=�6�   $
 Release=�6�   6   �� 5v      -   5        $	 SaveAddService��5v      $��
200,0,0,10q�6-   $y�	Collapsed=�6A   #��[ 6X   $��Сохранить��6�   $
��Left=�6�   $	��150��6�   ���	#FF0017FF$��32��6�   $��Bottom=�6�   $
 Release=�6  6  �� 5w      -   5        $	 DelAddService��5w      $��409,0,425,9.6q�6,   #��[ 6C   $	��150��6u   $��Удалить��6�   ���	#FF0017FF$��32��6�   $��Bottom=�6�   $��Center=�6�   $
 Release=�6�   6  �� 5x      -   5        $	 	SalesView��5x      $��0,0,200,9.6q�6(   #��[ 6=   $��Список продаж��6o   $��Right=�6�   $	��180��6�   ���	#FF0017FF$��32��6�   $��Bottom=�6�   $
 Release=�6�   6  �� 5y      -   5        $	 NextBtn��5y      $��	0,0,50,10q�6&   #��[ 69   $��>��6~   $��Right=�6�   $��32��6�   $��32��6�   $��Bottom=�6�   $
 Release=�6�   6  5z      5{      5}      