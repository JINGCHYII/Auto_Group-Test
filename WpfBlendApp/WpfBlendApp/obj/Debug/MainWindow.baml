   M S B A M L   `   `   `  ����F  BWpfBlendApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:WpfBlendAppWpfBlendApp  5   	       WpfBlendApp.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:WpfBlendApp     5   	   -   5          �Name$  myWindow��5   	    � Title$ 
MainWindow��6   $	��450��6.   $	��800��6;    � 	Resources 5
         WindowColor  ContentAreaColorLight  ContentAreaColorDark  DisabledControlLightColor  DisabledControlDarkColor  DisabledForegroundColor  SelectedBackgroundColor  SelectedUnfocusedColor  ControlLightColor 	 ControlMediumColor 
 ControlDarkColor  ControlMouseOverColor  ControlPressedColor  
GlyphColor  GlyphMouseOver  BorderLightColor  BorderMediumColor  BorderDarkColor  PressedBorderLightColor  PressedBorderDarkColor  DisabledBorderLightColor  DisabledBorderDarkColor  DefaultBorderBrushDarkColor  HeaderTopColor " DatagridCurrentCellBorderColor  SliderTrackDarkColor  NavButtonFrameColor  MenuPopupBrush �� EndPoint �� 
StartPoint � Color � Offset $  ProgressBarIndicatorAnimatedFill  aaa �� 
TargetType �� Property	 �� Value
 �� 
TargetType ��Name �� Color   WpfBlendApp.MyListBox  Name � DataContext   LCsProperty  ListBoxStyle1  ItemsPanelTemplate1 �� Left
 �� Top �� Triggers P� Property P� Value �� 
TargetName��%
  &	        &	 !     &	 B     &	 c     &	 �     &	 �     &	 �     &	 �     &	     &		 %    &	
 F    &	 g    &	 �    &	 �    &	 �    &	     &	 .    &	 O    &	 p    &	 �    &	 �    &	 �    &	 �    &	     &	 2    &	 O    &	 p    &	 �    &	 X    &	 �    7  ��5   
   	#FFE8EDF9��6/   ��5   
   	#FFC5CBF9��69   ��5   
   	#FF7381F9��68   ��5   
   	#FFE8EDF9��6=   ��5   
   	#FFC5CBF9��6<   ��5   
   	#FF888888��6;   ��5   
   	#FFC5CBF9��6;   ��5   
   	#FFDDDDDD��6:   ��5   
   	White��61   ��5   
   	#FF7381F9��66   ��5   
   	#FF211AA9��64   ��5   
   	#FF3843C4��69   ��5   
   	#FF211AA9��67   ��5   
   	#FF444444��6.   ��5    
   /+sc#1, 0.004391443, 0.002428215, 0.242281124��6T   ��5#   
   	#FFCCCCCC��64   ��5$   
   	#FF888888��65   ��5%   
   	#FF444444��63   ��5'   
   	#FF888888��6;   ��5(   
   	#FF444444��6:   ��5*   
   	#FFAAAAAA��6<   ��5+   
   	#FF888888��6;   ��5-   
   	Black��6;   ��50   
   	#FFC5CBF9��62   ��51   
   	Black��6>   ��52   
   	#FFC5CBF9��68   ��54   
   	#FF3843C4��67   �� 56   
   $ 0.5,1&�57      $ 0.5,0&�58      .��59      � # �  6   $ 0Q�5:      � 5;      # � 	 6   $	 0.5Q�5<      � 5=      # �  6   $ 1Q�5>      5?      �� 5A   
   $	 0,0&�5B      $	 1,0&�5C      ��5E      � � 5F      $ 	#000000FF��6#   $ 0Q�5G      � 5H      $ 	#600000FF��6#   $	 0.4Q�5I      � 5J      $ 	#600000FF��6#   $	 0.6Q�5K      � 5L      $ 	#000000FF��6#   $ 1Q�5M      5N      5P      �� 5R   
   " ��6   .�5S      ��  �@J�SnapsToDevicePixels	 . 5T      �� 5U       �@�OverridesDefaultStyle	 . 5V      �� 5W       �@�IsSharedSizeScope	 . 5X      �� 5Y       �@��HasDropShadow	 . 5Z      �� 5[       � ��	 5\      �� 5]      "
 ��6&   .R�5^      �� $ Border��6!   $��80��61   $	��120��6<   ���	#FF464646$��1q�5`      ��5a      �� 5b   "   8   62   ._�5f        $ 	UCListBox��6.   $��80��6A   $	��120��6L   # z 6Y   #���  6�   #���  6�   $	 160��6�   $ 92��6�   $��Hidden=�6  $��Disabled=�66  5g       5h      P� 5i       �@��HasDropShadow . . �5k   "   �� $ Border��6)    �@��Padding$	 0,3,0,3q�5m      �� 5n   "   $ Border��6)    �@��CornerRadius$	 4��5p      5q      5s      5u      5v      5w      .��5~      � -   5         �Name$ mainGrid��5~       5�   
      WpfBlendApp.BindingProxy 	  Proxy   Data��%&   &	          5�       6/   �� 5�       � ContextMenu 5�   
   �� 5�      -   5         ��Name$ contextmenu��5�      #��[ 6/   ��6L   $   WpfBlendApp.EnumerateExtension  *M� *local:UCType++5�      .+�5�   
   �� -   5         ��Name$ scrollViewer��5�      $��Hidden=�6-   $��Hidden=�6T   .��5�      � -   5        $ 
scrollGrid��5�      $	��800��6'   $	��450��63    J� RenderTransformOrigin$ 0.5,0.5&�6@    � LayoutTransform 5�      V� 5�      .~�5�      �� -   5        scaleTransform��5�   )   5�      .+�5�      �� -   5         ��Name$ canvas��5�      
�������$ 0.5,0.5&�6;   �� 5�      -   5        $ canvas2��5�      
������ $ 0.5,0.5&�6C   .+�5�      �� -	   5         ��Name$ SelRect��5�       ���	#FFB0B0B0 �� StrokeDashArray$ 3R�6D   $	��1.5��6X     J� IsHitTestVisible  .  6n   �� 5�      -
   5        ! ��Name$! StartContent��5�   %   .��5�      � -   5        $ dragGrid��5�      
������ $��92��6J   $��92��6V   .+�5�      �� -   5        $ dragRec��5�   (   $��88��69   $	��Top=�6d   $
��89.1��6|   " �� RadiusY$" 4.996��6�   # �� RadiusX$# 4.996��6�   ���	#FF464646$��1,4,2,0q�6�   ��6E   �� *BorderColor+6�   �� 5�      -   5        $ ��Name$$ startBtn��5�   %   $
��Test��67   $
��Left=�6F   $��5,26,0,0q�6a   $	��Top=�6s   $��63��6�   $��19��6�   ���	#FF5959596�   �� 5�      -   5        $$ stopBtn��5�   %   $
��Stop��66   $
��Left=�6E   $��5,47,0,0q�6`   $	��Top=�6r   $��63��6�   $��19��6�   ���	#FF5959596�   �� 5�      -   5        $$ waitBtn��5�   %   $
��Wait��66   {�.  $
��Left=�6W   $��5,68,0,0q�6r   $	��Top=�6�   $��63��6�   $��19��6�   ���	#FF5959596�   A� 5�      -   5        % A�Name$	% SRG��5�   &   $��11��63   
���   �$��11��6N   $��
79,35,2,10q�6Z   ��5�   "   �� 5�   &   " A�6,   .�5�   *   ��  � ��	 �	#FF8989896A   & �� Triggers& 5�   *   ^� 5�   .   ' ^� 
Conditions' 5�   2   �� 5�   6   ( �� Binding( 6@   �� ) �� RelativeSource) �� * �� Mode$
* Self=�+ �� Path$+ IsMouseOver�, �� Value, True6�   �� 5�   6   ( 6@   �� *GridColorTrigger_L+, True6g   .0�5�   2   ��  � ��	 �	#FFB4B93F6I   5�   /   5�   '   5�      F� 5�      -   5        - F�Name$- divPath��5�   #   "����H�� șQ�D�� șQ���șQ	���	#FFAEAEAE$��1��6~   $��1.456,22.125,2,0q�6�   . �� Stretch$
. Fill=�6�   
���   �$	��Top=�6�   F� 5�      -   5        $- dragPath��5�   #   / � Cursor$
/ Hand��65   ����� � ��S S � �\i���@�\i���@ � ��S  � � � � �O! � �B%�\i���@�B%��S S�B%(����� @�B%�� �B%�� ��B%��  � ��  � �� � � ��  � (����� @ � ��S 	���	#FF646464$��22.467��5�   2   $��1,0.283,2,0q�6B   $
. Fill=�6W   $	��Top=�6�   ��6f   �� *BorderColor+5�   q   �� 5�      $��Start��6(     .  $��5,4,5,75q�6N   ���	#FFD8D8D86`   5�      5�        5�      -   5        $ 	UCListBox��5�   &   $��80��69   $	��120��6D   #���  6Q   #���  6y   $	 160��6�   $ 92��6�   $��Hidden=�6�   $��Disabled=�6  5�      5�      5�      5�      5�      