   M S B A M L   `   `   `  ���� U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf   SolarDataAnalytics%   SolarEnergyAnalytics.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf       � Title$  Solar Energy Dashboard��$	��800��$
��1000��.��� $��10q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+���  �� Orientation$ 
Horizontal=�$��0,0,0,10q�.+��� -   $��Load Default Dataset��$	��150���� -   $��Browse Dataset...��$	��150��$��10,0,0,0q�� -    �Name$ DataLoadProgressBar��$	��150��$��20��$��10,0,0,0q�$y�	Collapsed=�� -   $ ChartRenderProgressBar��$	��150��$��20��$��10,0,0,0q�$y�	Collapsed=� � IsIndeterminate . &   System.Windows.Controls.DataGrid  -     Name$ DataGridView��$��1��$	��200��   AutoGenerateColumns . $��0,0,0,10q��� $��2��.@��� $��Line Chart - Daily Yield��.��#   LiveCharts.Wpf.CartesianChart  -     Name$ 	LineChart��&    LiveCharts.Wpf.Charts.Base.Chart   AxisX    LiveCharts.Wpf.Axis  	   Title$
	 Date��
   AxisY
   $	 Yield (kWh)���� $��Bar Chart - Avg Power��.��  -   $ BarChart��   $
	 Date��
   $	 Power���� $'��!Pie Chart - AC Power Distribution��.��   LiveCharts.Wpf.PieChart  -     Name$ PieChart���� $0��*DC Power to AC Power Conversion Percentage��.��  -	   $ 	LossChart��   $
	 Date��
   $	 % Conversion��   LabelFormatter �� *$"ConversionPercentageLabelFormatter+�� $��DC Power for Sources��.��� .+�  -
   $ DcPowerChart1��$��0��   $	 Time of Day��
   $	 DC POWER (kW)��  -   $ DcPowerChart2��$��1��   $	 Time of Day��
   $	 DC POWER (kW)��