   M S B A M L   `   `   `  ����K  GMegaXConfigTool, Version=1.1.2.13, Culture=neutral, PublicKeyToken=null1clr-namespace:MegaXConfigToolMegaXConfigTool  5      G(clr-namespace:MegaXConfigTool.PropertiesMegaXConfigTool.Properties  5      1clr-namespace:MegaXConfigToolMegaXConfigTool  5      U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5	      >/clr-namespace:ControlzEx;assembly=MahApps.Metro
ControlzEx 6d   !    MegaXConfigTool.FileManager   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F BNHotkey.Wpf, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      /localclr-namespace:MegaXConfigTool     5      6p(clr-namespace:MegaXConfigTool.Properties     5      9MegaXConfigToolclr-namespace:MegaXConfigTool     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5	      F
ControlzEx/clr-namespace:ControlzEx;assembly=MahApps.Metro     6d   -   5        (  "MahApps.Metro.Controls.MetroWindow   Name$  
BaseWindow��5	   �    � Title$ Mega-X Config Tool��5      $	��510��62   $	��650��6?    � Icon$ /images/icon.ico��6K    � WindowStartupLocation$ CenterScreen=�5   T   $��Visible=�6y    � WindowStyle$
 None=�6�    � AllowsTransparency .    ShowTitleBar .     IsWindowDraggable .  � 
ResizeMode$ NoResize=�5   z      ButtonStyle	   WindowCloseButtonStyle#	 �   6�   
 � 	Resources
 5      $   MegaXConfigTool.defaultDataSet  databaseDataSet  clientInfoViewSource �� Source �� Source �� 
TargetType  MetroWindowButtonStyle �� BasedOn �� Triggers P� Property P� Value �� Property �� Value��%m  &	       &	      7  &	  A     7    5   
   6)   �� 5   
    6<   �� *
MainInfo+8   �� 5   
   " ��6$   8    5      P� 5       �@J�IsMouseOver . . �5      ��  � �� 63   Z� �� 5       � ��
 �����63   5      P� 5       � �� . . �5      ��  �@J�Opacity$ 1Q�60   5      5       5!      .��5F      � .+�5I   
   ]� ���	#FF444444$	��Top=�6+   $��40��6C    J� Opacity$	 0.9Q�6O   .+�5K      !  MahApps.Metro.Controls.Tile  -   5          Name$ NewTile��5K      $	��200��6.   $��34��6:   *   $MegaXConfigTool.Properties.Resources   NewConfigFile   Title# Z 6F      TitleFontSize$ 12Q�6s      HorizontalTitleAlignment$ Right=�6�   $��Center=�5L      
�������$
��Left=�6j   
���   �$
��Bold"�6�   .��5M      �� $%��/images/toptiles/config_new.png��6   $��	-60,0,0,0q�6A   5N        5O      -   5        $ 
DeleteTile��5O      $��0,0,0,0q�60   $	��200��6A   $��34��6M      DeleteConfigFile# Z 6Y   $ 12Q�6�   $ Right=�6�   $��Center=�5P      
�������$
��Left=�6m   
���   �$
��Bold"�6�   .��5Q      �� $(��"/images/toptiles/config_delete.png��6   $��	-60,0,0,0q�6D   5S        5g      -   5        $ HelpTile��5g      $��32��6.   $��32��69   $��Right=�6E   $	��Top=�6a   $
��30,0q�6z     
BlackBrush+  %MahApps.Metro.Controls.ControlsHelper   MouseOverBorderBrush# �  6�   
���   � � Cursor$
 Hand��5h   p   #���   6~   ��60   Z� .��5i      ��  �� Orientation$ 
Horizontal=�6   .+�5j      �� $"��/images/topbaricons/help.png��6   $��16��6B   $��16��6M   $��Center=�6Y   $��Right=�6t   5k      5m      5n      ]� 5p   
   -   5         ]�Name$ 	BaseFrame��5p      $��	0,40,0,35q�6'   ��.  $��Disabled=�6`   
�������.+�5q      �� -   5         ��Name$ 	DBListBox��5q      $
��Auto��6+   $
��Left=�68   
�������$��1q�6k   
�������%  ControlzEx.KeyboardNavigationEx    AlwaysShowFocusVisual  .  ��5r      �� 5s      " ��6   .�5t      -   5        5u      ! �� View! 5w      �� 5x      " �� AllowsColumnReorder" .  .P�5y      �� # �� Width$	# 100��6O   $ �� DisplayMemberBinding$ 6)   �� *Name+��5z      �� 5{   "   %   Description#��Z% 6,   $
��Wrap=�6V   5}      �� 5~      $
# Auto��6}   $ 6)   �� *LastChanged+& �� StringFormat$& {0:MM/dd/yyyy H:MM }����5      �� 5�   "   '   LastChanged#��Z' 6,   $
��Wrap=�6V   5�      5�      5�      � 5�      
�������( � DataContext#( [ 6*   $��	-10,0,0,0q�6^   .+�5�      �� .+�5�      �� )   
InfoClient#��Z) 6   $��10,10q�6H   $��22(�6W   �� 5�      -   5        * ��Name$* ProjectName��5�       $��10(�65   
�������$��
15,-15,0,0q�6U   �� 5�      $
��Left=�6!   $��Center=�6<   $
��Auto��6W   $
��Auto��6e   $��0,0,30,0q�6r   .+�5�      �� $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   +   
ClientName#��Z+ 62   $	��120��6^   $
��Bold"�6j   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -	   5        $* 
ClientName��5�   ,   $��Stretch=�6@   $	��350��6\   $��30��6h   $	��0,7q�6t   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   ,   Address1#��Z, 63   $	��120��6]   $��Black"�6i   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -
   5        $* ClientAddr1��5�   ,   $��Stretch=�6A   $	��350��6]   $��30��6i   $	��0,7q�6u   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   -   Address2#��Z- 62   $	��120��6\   $��Black"�6h   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -   5        $* ClientAddr2��5�   ,   $��Stretch=�6A   $	��350��6]   $��30��6i   $	��0,7q�6u   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   .   City#��Z. 62   $	��120��6X   $��Black"�6d   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -   5        $* 
ClientCity��5�   ,   $��Stretch=�6@   $	��350��6\   $��30��6h   $	��0,7q�6t   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   /   
PostalCode#��Z/ 63   $	��120��6_   $��Black"�6k   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -   5        $* ClientPostalCode��5�   ,   $��Stretch=�6F   $	��350��6b   $��30��6n   $	��0,7q�6z   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   0   District#��Z0 63   $	��120��6]   $��Black"�6i   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -   5        $* ClientDistrict��5�   ,   $��Stretch=�6D   $	��350��6`   $��30��6l   $	��0,7q�6x   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   1   Phone#��Z1 63   $	��120��6Z   $��Black"�6f   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -   5        $* ClientPhone��5�   ,   $��Stretch=�6A   $	��350��6]   $��30��6i   $	��0,7q�6u   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   2   	FaxNumber#��Z2 62   $	��120��6]   $��Black"�6i   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -   5        $* 	ClientFax��5�   ,   $��Stretch=�6?   $	��350��6[   $��30��6g   $	��0,7q�6s   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   3   Email#��Z3 63   $	��120��6Z   $��Black"�6f   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -   5        $* ClientEmail��5�   ,   $��Stretch=�6A   $	��350��6]   $��30��6i   $	��0,7q�6u   
�������6�   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   .+�5�      �� $
��10,5q�6$   4   Website#��Z4 63   $	��120��6\   $��Black"�6h   �� 5�      $��30��6<   ��6%   Z� ._�5�   "   �� -   5        $* ClientWebSite��5�   ,   $��Stretch=�6C   $	��350��6_   $��30��6k   $	��0,7q�6w   
�������6�   5�      5�      5�      5�      5�      5�      ]� 5�   
   $��34��6   $��Bottom=�6    
�������.+�5�      �� -   5        5 ��Name$5 show_version��5�      $��5q�6*   $
��Bold"�65   
�������$��10(�6Y   �� 5�      $��/images/maxalarm.png��6   $
��Auto��62   $��30��6?   $��Right=�6K   $
��10,0q�6g   5�      5      5     