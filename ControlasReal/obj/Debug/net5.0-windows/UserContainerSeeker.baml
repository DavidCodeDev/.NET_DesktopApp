   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35X@clr-namespace:System.Windows.Data;assembly=PresentationFrameworkSystem.Windows.Data  5      H DControlasReal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ControlasRealControlasReal 5      '   !ControlasReal.UserContainerSeeker   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      5      @d2http://schemas.microsoft.com/expression/blend/2008      5      Qdata@clr-namespace:System.Windows.Data;assembly=PresentationFramework      5      -localclr-namespace:ControlasReal      5      .��5
      � 
�������  � RowDefinitions  5   
   �� 5      $��75 �6   6*   �� 5      $
��Auto �6   6,   .+�5   
   �� -   5         ��Name$ MainContent��5      $��0,10,0,-475q�6.   $��2��6C   � 5   
   $��1��6   $��
0,0,0,-475q�6   .+�5      �� $��Visible=�6   $��0,0,0,0q�6A   .��5      &    System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5         AutoGenerateColumns$
 True��6�   $��142,55,142,55q�6�    J� RenderTransformOrigin$ 0.5,0.5&�6�      SelectionMode$ Single=�6�      
IsReadOnly$
 True��6  ��6)   �� *Users+ �� RelativeSource ��  �� AncestorType'  !ControlasReal.UserContainerSeeker"  z�5      V� 5      .~�5      �� �� 5      	 �� AngleY$	 0Q�6(   �� 5      
 �� Angle$
 0Q�6*   T� 5       T� X$ 0Q�6-    T� Y$ 0Q�63   5      5      5      5      �� 5   
   -   5         ��Name$ SearchTextBox��5      $��1��6)   $��143,35,617,-55q�66   $��20��6N   5       5!      5"      