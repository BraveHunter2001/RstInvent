using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using System.IO;
using RstInvent.Model;
using System.Linq;

namespace RstInvent
{
    internal static class CsvUtil
    {
        public static List<NomenclatureEntity> GetNomenclatureEntities()
        {
            try
            {
                using (var reader = new StreamReader("Nomenclature.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var entities = csv.GetRecords<NomenclatureEntity>().ToList();
                    return entities;
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static void SaveNomenclatureEntities(List<NomenclatureEntity> entities)
        {
            using (var writer = new StreamWriter("Nomenclature.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(entities);
            }
        }
    }
}