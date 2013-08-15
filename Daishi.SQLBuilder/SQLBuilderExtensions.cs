﻿#region Includes

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Daishi.SQLBuilder {
    public static class SQLBuilderExtensions {
        public static SQLBuilder Select(this SQLBuilder source, params string[] columnNames) {
            source.Command.CommandText = string.Concat(@"select ", string.Join(@",", columnNames), source.Command.CommandText);
            return source;
        }

        public static SQLBuilder Select(this SQLBuilder source, IEnumerable<string> columnNames) {
            source.Command.CommandText = string.Concat(@"select ", string.Join(@",", columnNames), source.Command.CommandText);
            return source;
        }

        public static SQLBuilder Select(this SQLBuilder source, params SQLParameter[] parameters) {
            var formatter = new SQLParameterFormatter(parameters);
            formatter.Execute();

            source.Command.Parameters = parameters.Select(p => p.Parameter).ToArray();
            source.Command.CommandText = string.Concat(formatter.Result, source.Command.CommandText);

            return source;
        }

        public static SQLBuilder Select(this SQLBuilder source, IEnumerable<SQLParameter> parameters) {
            var @params = parameters.ToList();

            var formatter = new SQLParameterFormatter(@params);
            formatter.Execute();

            source.Command.Parameters = @params.Select(p => p.Parameter).ToArray();
            source.Command.CommandText = string.Concat(formatter.Result, source.Command.CommandText);

            return source;
        }

        public static SQLBuilder From(this SQLBuilder source, params string[] tableNames) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" from ", string.Join(@",", tableNames.Select(tn => string.Concat(@"dbo.", tn))));
            return source;
        }

        public static SQLBuilder Insert(this SQLBuilder source, string tableName, IEnumerable<string> parameters, params object[] values) {
            var stringBuilder = new StringBuilder(@" values (");
            var formattedValues = new List<string>();

            foreach (var value in values) {
                if (value is int) formattedValues.Add(value.ToString());
                else formattedValues.Add(string.Concat(@"'", value, @"'"));
            }

            stringBuilder.Append(string.Join(@",", formattedValues));
            stringBuilder.Append(@")");

            source.Command.CommandText = string.Concat(source.Command.CommandText, @"insert dbo.", tableName, @" (", string.Join(@",", parameters), @")", stringBuilder);
            return source;
        }

        public static SQLBuilder Delete(this SQLBuilder source) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @"delete");
            return source;
        }

        public static SQLBuilder Where(this SQLBuilder source, string columnName) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" where ", columnName);
            return source;
        }

        public static SQLBuilder Where(this SQLBuilder source, string tableName, string columnName) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" where ", tableName, @".", columnName);
            return source;
        }

        public static SQLBuilder EqualTo(this SQLBuilder source, int identifier) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @"=", identifier);
            return source;
        }

        public static SQLBuilder EqualTo(this SQLBuilder source, string identifier) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @"=", string.Concat(@"'", identifier, @"'"));
            return source;
        }

        public static SQLBuilder NotEqualTo(this SQLBuilder source, int identifier) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @"!=", identifier);
            return source;
        }

        public static SQLBuilder NotEqualTo(this SQLBuilder source, string identifier) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @"!=", string.Concat(@"'", identifier, @"'"));
            return source;
        }

        public static SQLBuilder In(this SQLBuilder source, params int[] constraints) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" in (", string.Join(@",", constraints), @")");
            return source;
        }

        public static SQLBuilder In(this SQLBuilder source, IEnumerable<int> constraints) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" in (", string.Join(@",", constraints), @")");
            return source;
        }

        public static SQLBuilder In(this SQLBuilder source, params string[] constraints) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" in (", string.Join(@",", constraints.Select(c => string.Concat(@"'", c, @"'"))), @")");
            return source;
        }

        public static SQLBuilder In(this SQLBuilder source, IEnumerable<string> constraints) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" in (", string.Join(@",", constraints.Select(c => string.Concat(@"'", c, @"'"))), @")");
            return source;
        }

        public static SQLBuilder And(this SQLBuilder source, string columnName) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" and ", columnName);
            return source;
        }

        public static SQLBuilder Or(this SQLBuilder source, string columnName) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" or ", columnName);
            return source;
        }

        public static SQLBuilder InnerJoin(this SQLBuilder source,
                                           string rightTableName,
                                           string leftTableName,
                                           string leftColumnName,
                                           string rightColumnName
            ) {
            source.Command.CommandText = string.Concat(source.Command.CommandText, @" inner join dbo.",
                                                       rightTableName, @" on dbo.", leftTableName, @".",
                                                       leftColumnName, @"=dbo.", rightTableName, @".", rightColumnName);
            return source;
        }
    }
}