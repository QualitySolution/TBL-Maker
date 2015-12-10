using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Ionic.Zip;
using Ionic.Zlib;
using Microsoft.Win32;

namespace TBLMaker
{
    internal static class ZipWriter
    {
        public static bool WriteZip(TBLFile file, string zipPath)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var dirInfo = Directory.CreateDirectory(tempFolder);
            if (!dirInfo.Exists) return false;
            foreach (var record in file.ParsedTBLRecords)
            {
                FileInfo fileInfo = null;
                if (!string.IsNullOrWhiteSpace(record.FilePath)) fileInfo = new FileInfo(record.FilePath);
                if (fileInfo == null || !fileInfo.Exists)
                {
                    var result = MessageBox.Show(
                        "Файл " + record.FileName + " не найден. Пожалуйста, укажите его местоположение.",
                        "Файл не найден!",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (result != MessageBoxResult.OK)
                    {
                        return false;
                    }
                    var openDlg = new OpenFileDialog {Multiselect = false, Filter = String.Format("Файл {0}|{0}", record.FileName)};
                    if (openDlg.ShowDialog() == true)
                    {
                        record.FilePath = openDlg.FileName;
                    }
                }
                var destFile = Path.Combine(tempFolder, record.FileName);
                try
                {
                    File.Copy(record.FilePath, destFile);
                }
                catch (IOException)
                {
                    MessageBox.Show(
                        "Не удалось сохранить *.zip архив. Скорее всего это связано с ошибками в *.tbl файле. " +
                        "К примеру для двух разных разделов может быть указан один файл прошивки. " +
                        "Проверьте правильность его формирования и повторите попытку. ",
                        "Ошибка сохранения!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
            }
            var dirBackup = file.Path;
            file.Path = Path.Combine(tempFolder, Path.GetFileName(file.Path));
            TBLWriter.Write(file);
            using (var zip = new ZipFile {CompressionLevel = CompressionLevel.None})
            {
                zip.AddDirectory(tempFolder);
                zip.Save(zipPath);
            }
            file.Path = dirBackup;
            return true;
        }
    }
}
