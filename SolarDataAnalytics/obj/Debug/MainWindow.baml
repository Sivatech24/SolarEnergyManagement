   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	    SolarDataAnalytics%   SolarEnergyAnalytics.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	     � Title$  Solar Energy Dashboard��5   	   $	��800��6(   $
��1000��65   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�6.   .+�5      �� -   5        $��Load Default Dataset��5      $	��150��64   6@   �� 5      -   5        $��Browse Dataset...��5      $	��150��61   $��10,0,0,0q�6=   6O   � 5      -   5         �Name$ DataLoadProgressBar��5      $	��150��67   $��20��6C   $��10,0,0,0q�6O   $y�	Collapsed=�6a   � 5      -   5        $ ChartRenderProgressBar��5      $	��150��6:   $��20��6F   $��10,0,0,0q�6R   $y�	Collapsed=�5       � IsIndeterminate . 61   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ DataGridView��5      $��1��6)   $	��200��66      AutoGenerateColumns . $��0,0,0,10q�6^   �� 5   
   $��2��6   .@�5      �� $��Line Chart - Daily Yield��6   .��5      #   LiveCharts.Wpf.CartesianChart  -   5          Name$ 	LineChart��5   %   &    LiveCharts.Wpf.Charts.Base.Chart   AxisX 5         LiveCharts.Wpf.Axis  	   Title$
	 Date��6#   
   AxisY
 5"        $	 Yield (kWh)��6#   5$      5%      �� 5'      $��Bar Chart - Avg Power��6   .��5(        -   5        $ BarChart��5(   %    5*        $
	 Date��6#   
 5-        $	 Power��6#   5/      50      �� 52      $'��!Pie Chart - AC Power Distribution��6   .��53         LiveCharts.Wpf.PieChart  -   5          Name$ PieChart��53      54      �� 55      $0��*DC Power to AC Power Conversion Percentage��6   .��56        -	   5        $ 	LossChart��56   %    58        $
	 Date��6#   
 5;        $	 % Conversion��6#      LabelFormatter 68   �� *$"ConversionPercentageLabelFormatter+5=      5>      �� 5?      $��DC Power for Sources��6   .��5@      � .+�5B        -
   5        $ DcPowerChart1��5B   )   $��0��6@    5D        $	 Time of Day��6'   
 5G        $	 DC POWER (kW)��6'   5I        5L      -   5        $ DcPowerChart2��5L   )   $��1��6@    5N        $	 Time of Day��6'   
 5Q        $	 DC POWER (kW)��6'   5S      5T      5U      5V      5W      5X      