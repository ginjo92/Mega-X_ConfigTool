   M S B A M L   `   `   `  ���� J  FMegaXConfigTool, Version=1.1.2.4, Culture=neutral, PublicKeyToken=null1clr-namespace:MegaXConfigToolMegaXConfigTool  G(clr-namespace:MegaXConfigTool.PropertiesMegaXConfigTool.Properties  1clr-namespace:MegaXConfigToolMegaXConfigTool  U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls >/clr-namespace:ControlzEx;assembly=MahApps.Metro
ControlzEx !    MegaXConfigTool.FileManager   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F BNHotkey.Wpf, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:MegaXConfigTool     6p(clr-namespace:MegaXConfigTool.Properties     9MegaXConfigToolclr-namespace:MegaXConfigTool     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     F
ControlzEx/clr-namespace:ControlzEx;assembly=MahApps.Metro     -   (  "MahApps.Metro.Controls.MetroWindow   Name$  
BaseWindow�� � Title$ Mega-X Config Tool��$	��510��$	��650�� � Icon$ /images/icon.ico�� � WindowStartupLocation$ CenterScreen=�$��Visible=� � WindowStyle$
 None=� � AllowsTransparency .    ShowTitleBar .     IsWindowDraggable .  � 
ResizeMode$ NoResize=�   ButtonStyle	   WindowCloseButtonStyle#	 �   
 � 	Resources
 $   MegaXConfigTool.defaultDataSet  databaseDataSet  clientInfoViewSource �� Source �� Source �� 
TargetType  MetroWindowButtonStyle �� BasedOn �� Triggers P� Property P� Value �� Property �� Value��%�   &	       &	      7  &	  %     7    ��  �� *
MainInfo+8   �� " ��8    P�  �@J�IsMouseOver . . ���  � �� Z� ��  � ��
 �����P�  � �� . . ���  �@J�Opacity$ 1Q�.��� .+�]� ���	#FF444444$	��Top=�$��40�� J� Opacity$	 0.9Q�.+�!  MahApps.Metro.Controls.Tile  -     Name$ NewTile��$	��200��$��34��*   $MegaXConfigTool.Properties.Resources   NewConfigFile   Title# Z    TitleFontSize$ 12Q�   HorizontalTitleAlignment$ Right=�$��Center=�
�������$
��Left=�
���   �$
��Bold"�.���� $%��/images/toptiles/config_new.png��$��	-60,0,0,0q�  -   $ 
DeleteTile��$��0,0,0,0q�$	��200��$��34��   DeleteConfigFile# Z $ 12Q�$ Right=�$��Center=�
�������$
��Left=�
���   �$
��Bold"�.���� $(��"/images/toptiles/config_delete.png��$��	-60,0,0,0q�  -   $ HelpTile��$��32��$��32��$��Right=�$	��Top=�$
��30,0q�  
BlackBrush+  %MahApps.Metro.Controls.ControlsHelper   MouseOverBorderBrush# �  
���   � � Cursor$
 Hand��#���   ��Z� .����  �� Orientation$ 
Horizontal=�.+��� $"��/images/topbaricons/help.png��$��16��$��16��$��Center=�$��Right=�]� -    ]�Name$ 	BaseFrame��$��	0,40,0,35q���.  $��Disabled=�
�������.+��� -    ��Name$ 	DBListBox��$
��Auto��$
��Left=�
�������$��1q�
�������%  ControlzEx.KeyboardNavigationEx    AlwaysShowFocusVisual  .  ���� " ��.�-   ! �� View! �� " �� AllowsColumnReorder" .  .P��� # �� Width$	# 100��$ �� DisplayMemberBinding$ �� *Name+���� %   Description#��Z% $
��Wrap=��� $
# Auto��$ �� *LastChanged+& �� StringFormat$& {0:MM/dd/yyyy H:MM }������ '   LastChanged#��Z' $
��Wrap=�� 
�������( � DataContext#( [ $��	-10,0,0,0q�.+��� .+��� )   
InfoClient#��Z) $��10,10q�$��22(��� -   * ��Name$* ProjectName��$��10(�
�������$��
15,-15,0,0q��� $
��Left=�$��Center=�$
��Auto��$
��Auto��$��0,0,30,0q�.+��� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�+   
ClientName#��Z+ $	��120��$
��Bold"��� $��30����Z� ._��� -	   $* 
ClientName��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�,   Address1#��Z, $	��120��$��Black"��� $��30����Z� ._��� -
   $* ClientAddr1��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�-   Address2#��Z- $	��120��$��Black"��� $��30����Z� ._��� -   $* ClientAddr2��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�.   City#��Z. $	��120��$��Black"��� $��30����Z� ._��� -   $* 
ClientCity��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�/   
PostalCode#��Z/ $	��120��$��Black"��� $��30����Z� ._��� -   $* ClientPostalCode��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�0   District#��Z0 $	��120��$��Black"��� $��30����Z� ._��� -   $* ClientDistrict��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�1   Phone#��Z1 $	��120��$��Black"��� $��30����Z� ._��� -   $* ClientPhone��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�2   	FaxNumber#��Z2 $	��120��$��Black"��� $��30����Z� ._��� -   $* 	ClientFax��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�3   Email#��Z3 $	��120��$��Black"��� $��30����Z� ._��� -   $* ClientEmail��$��Stretch=�$	��350��$��30��$	��0,7q�
��������� $ 
Horizontal=�$��Center=�.+��� $
��10,5q�4   Website#��Z4 $	��120��$��Black"��� $��30����Z� ._��� -   $* ClientWebSite��$��Stretch=�$	��350��$��30��$	��0,7q�
�������]� $��34��$��Bottom=�
�������.+��� -   5 ��Name$5 show_version��$��5q�$
��Bold"�
�������$��10(��� $��/images/maxalarm.png��$
��Auto��$��30��$��Right=�$
��10,0q�