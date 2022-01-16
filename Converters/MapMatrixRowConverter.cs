using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using Avalonia.Data.Converters;
using BeaterLibrary.Formats.Maps;

namespace SwissArmyKnife.Avalonia.Converters; 

public class MapMatrixRowConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        var matrix = value as ObservableCollection<MapMatrixRow>;
        if (matrix != null) {
            int nRows, nColumns;
            if ((nRows = matrix.Count) > 0 && (nColumns = matrix[0].Count) > 0) {
                DataTable t = new DataTable();
                for (int Index = 0; Index < nColumns; ++Index) {
                    t.Columns.Add(new DataColumn(Index.ToString()));
                }
                for (int Row = 0; Row < nRows; ++Row) {
                    DataRow newRow = t.NewRow();
                    for (int Column = 0; Column < nColumns; ++Column) {
                        newRow[Column] = matrix[Row][Column];
                    }
                    t.Rows.Add(newRow);
                }

                return t.DefaultView;
            }
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return null;
    }
}