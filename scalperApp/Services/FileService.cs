using scalperApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace scalperApp.Services
{
    public static class FileService
    {
        public static string SerializeUserCollection(UserCollectionModel userCollection)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[CollectionName]").AppendLine(userCollection.Name);

            sb.AppendLine("[Items]");
            foreach (var item in userCollection.Items)
            {
                sb.AppendLine("[Item]");
                sb.AppendLine(item.Name);
                sb.AppendLine(item.Price.ToString());
                sb.AppendLine(item.Condition);
                sb.AppendLine(item.WantToSell.ToString());
                sb.AppendLine(item.Sold.ToString());
                sb.AppendLine(item.ImgBlob);
            }

            return sb.ToString();
        }

        public static UserCollectionModel DeserializeUserCollection(string[] lines)
        {
            var userCollection = new UserCollectionModel("");

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "[CollectionName]")
                    userCollection.Name = lines[++i];

                else if (lines[i] == "[Item]")
                {
                    ItemModel item = new ItemModel();
                    item.Name = lines[++i];
                    float.TryParse(lines[++i], out float price);
                    item.Price = price;
                    item.Condition = lines[++i];
                    bool.TryParse(lines[++i], out bool wantToSell);
                    item.WantToSell = wantToSell;
                    bool.TryParse(lines[++i], out bool sold);
                    item.Sold = sold;
                    item.ImgBlob = lines[++i];
                    userCollection.Items.Add(item);
                }
            }

            if (string.IsNullOrEmpty(userCollection.Name))
                userCollection.Name = "Unnamed";

            return userCollection;
        }

        public static string SerializeCollectionList(CollectionsListModel collectionList)
        {
            var sb = new StringBuilder();

            foreach (var studentClass in collectionList.AllCollections)
            {
                sb.AppendLine("{");
                sb.Append(SerializeUserCollection(studentClass));
                sb.AppendLine("}");
            }
            return sb.ToString();
        }

        public static CollectionsListModel DeserializeCollectionList(string[] lines)
        {
            var collectionList = new CollectionsListModel();
            var classLineBuffer = new List<string>();
            foreach (var line in lines)
            {
                if (line == "{")
                {
                    classLineBuffer.Clear();
                }
                else if (line == "}")
                {
                    var userCollection = DeserializeUserCollection(classLineBuffer.ToArray());
                    collectionList.AllCollections.Add(userCollection);
                }
                else
                {
                    classLineBuffer.Add(line);
                }
            }
            return collectionList;
        }
    }
}
