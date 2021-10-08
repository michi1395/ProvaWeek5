using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProvaWeek5.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaWeek5
{
    public class DisconnectedMODE
    {
        static IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        static string connectionStringSQL = config.GetConnectionString("AcademyG");

        internal static void Start()
        {
            bool quit = false;
            do
            {
                string command = ConsoleHelpers.BuildMenu("Disconnected Menu",
                    new List<string> {
                    "[ 1 ] - Aggiungi Spesa",
                    "[ 2 ] - Cancella Spesa"
                    });
                switch (command)
                {
                    case "1":
                        AggiungiSpesa();
                        break;
                    case "2":
                       CancellaSpesa();
                        break;
                    default:
                        Console.WriteLine("Comando sconosciuto.");
                        break;
                }
            } while (!quit);
        }

        private static void CancellaSpesa()
        {
            throw new NotImplementedException();
        }

        private static void AggiungiSpesa()
        {
            DataSet speseDs = new DataSet();

            using SqlConnection conn = new SqlConnection(connectionStringSQL);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                    Console.WriteLine("Connection Open.");
                else
                {
                    Console.WriteLine("Cannot open connection.");
                    return;
                }

                SqlDataAdapter speseAdapter = InitSpeseDataSetAndAdapter(speseDs, conn);
                conn.Close();

                Console.Clear();
                Console.WriteLine("---- Inserire una nuova Spesa ----");

                DataRow newRow = speseDs.Tables["Spese"].NewRow(); 
                newRow["Data"] = DateTime.Now;
                newRow["CategoriaId"]= int.Parse(ConsoleHelpers.GetData("Id categoria"));
                newRow["Descrizione"]= ConsoleHelpers.GetData("Descrizione");
                newRow["Utente"]= ConsoleHelpers.GetData("Utente");
                newRow["Importo"]= decimal.Parse(ConsoleHelpers.GetData("Importo"));
                newRow["Approvazione"] = false;
                speseDs.Tables["Spese"].Rows.Add(newRow);
                

                speseAdapter.Update(speseDs, "Spese");

            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generic Exception: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        private static SqlDataAdapter InitSpeseDataSetAndAdapter(DataSet speseDs, SqlConnection conn, SqlTransaction speseTransaction = null)
        {
   
                SqlDataAdapter speseAdapter = new SqlDataAdapter();

                speseAdapter.InsertCommand = GenerateInsertCommand(conn, speseTransaction);    // UPDATE
                //speseAdapter.DeleteCommand = GenerateDeleteCommand(conn, speseTransaction);    // UPDATE

                // evita di dover definire a mano la PK nelle tabelle
                speseAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                speseAdapter.Fill(speseDs, "Spese");

                return speseAdapter;
            }

        private static SqlCommand GenerateDeleteCommand(SqlConnection conn, SqlTransaction customerTransaction)
        {
            throw new NotImplementedException();
        }

        private static SqlCommand GenerateInsertCommand(SqlConnection conn, SqlTransaction speseTransaction)
        {
            SqlCommand speseInsertCommand = new SqlCommand();
            speseInsertCommand.Connection = conn;
            
            DateTime data = DateTime.Now;
            string descrizione = ConsoleHelpers.GetData("Descrizione");
            string utente = ConsoleHelpers.GetData("Utente");
            decimal importo = decimal.Parse(ConsoleHelpers.GetData("Importo"));
            string categoria = ConsoleHelpers.GetData("Categoria");
            bool approvazione = false;

            speseInsertCommand.CommandType = CommandType.Text;
            speseInsertCommand.CommandText = "INSERT INTO Spese VALUES(@data, @categoria, @descrizione, @utente, @importo, @approvazione)";

            speseInsertCommand.Parameters.Add(new SqlParameter(
                "@data",
                SqlDbType.DateTime,
                10,
                "Data"
            ));

            speseInsertCommand.Parameters.Add(new SqlParameter(
                "@categoria",
                SqlDbType.Int,
                50,
                "CategoriaId"
            ));
            speseInsertCommand.Parameters.Add(new SqlParameter(
                "@descrizione",
                SqlDbType.NVarChar,
                500,
                "CategoriaId"
            ));

            speseInsertCommand.Parameters.Add(new SqlParameter(
                "@utente",
                SqlDbType.NVarChar,
                100,
                "Utente"
            ));
            speseInsertCommand.Parameters.Add(new SqlParameter(
                "@importo",
                SqlDbType.Decimal,
                10,
                "Importo"
            ));
            speseInsertCommand.Parameters.Add(new SqlParameter(
                "@approvazione",
                SqlDbType.Bit,
                5,
                "Approvazione"
            ));

            if (speseTransaction != null)
                speseInsertCommand.Transaction = speseTransaction;

            return speseInsertCommand;
        }
    }
    
}

