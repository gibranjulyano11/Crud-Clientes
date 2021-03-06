﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transacciones
{
    class ConexionPgsql
    {
        NpgsqlConnection conexion = new NpgsqlConnection("Server = localhost; User Id = postgres; Password = 1110; Database = Db_Transacciones;");

        public DataTable Consultar()
        {
            string query = "SELECT  *  FROM clientes where estado=true";
            NpgsqlCommand conector = new NpgsqlCommand(query, conexion);
            NpgsqlDataAdapter datos = new NpgsqlDataAdapter(conector);
            DataTable tabla = new DataTable();
            datos.Fill(tabla);
            return tabla;
        }

        public void Transferir(string textBox1, string textBox2, int textBox3)
        {
            try
            {
                conexion.Open();
                string query1 = "BEGIN";
                NpgsqlCommand ejecutor1 = new NpgsqlCommand(query1, conexion);
                ejecutor1.ExecuteNonQuery();

                string query2 = $"UPDATE clientes set saldos= saldos-{textBox3} WHERE cuenta = '{textBox2}' AND saldos>= {textBox3}";
                NpgsqlCommand ejecutor2 = new NpgsqlCommand(query2, conexion);
                var e = ejecutor2.ExecuteNonQuery();
                conexion.Close();

                string query3 = $"UPDATE clientes set saldos= saldos+ {textBox3} WHERE cuenta = '{textBox1}'";
                NpgsqlCommand ejecutor3 = new NpgsqlCommand(query3, conexion);
                var f = ejecutor3.ExecuteNonQuery();

                if (f == 1 && e == 1)
                {
                    string query4 = "COMMIT";
                    NpgsqlCommand ejecutor4 = new NpgsqlCommand(query4, conexion);
                    ejecutor4.ExecuteNonQuery();
                    conexion.Close();
                    MessageBox.Show("La transacción se realizó exitosamente.");
                }
                else
                {
                    string query4 = "ROLLBACK";
                    NpgsqlCommand ejecutor4 = new NpgsqlCommand(query4, conexion);
                    ejecutor4.ExecuteNonQuery();
                    conexion.Close();
                    MessageBox.Show("La transacción no se realizó.");
                }
            }
            catch (Exception)
            {
                conexion.Close();
                MessageBox.Show("¡Ha ocurrido un problema!, inténtalo nuevamente.");
            }
        }

        public void CrearCliente(string nombre , string direccion, string cuenta, int saldos)
        {
            conexion.Open();
            string query1 = $"select ps_funciones_crud (1, '{nombre}', '{direccion}', '{cuenta}', {saldos}, true)";
            NpgsqlCommand ejecutor1 = new NpgsqlCommand(query1, conexion);
            ejecutor1.ExecuteNonQuery();
            conexion.Close();
            MessageBox.Show("Sus datos se guardaron satisfactoriamente.");
        }

        public void EditarCliente(string nombre, string direccion, string cuenta, int saldos)
        {
            conexion.Open();
            string query1 = $"select ps_funciones_crud (2, '{nombre}', '{direccion}', '{cuenta}', {saldos}, true)";
            NpgsqlCommand ejecutor1 = new NpgsqlCommand(query1, conexion);
            ejecutor1.ExecuteNonQuery();
            conexion.Close();
            MessageBox.Show("Sus datos se editaron satisfactoriamente.");
        }

        public void EliminarCliente(string cuenta)
        {
            conexion.Open();
            var x = 0;
            string query1 = $"select ps_funciones_crud (3, 'xx', 'xx', '{cuenta}', {x}, false)";
            NpgsqlCommand ejecutor1 = new NpgsqlCommand(query1, conexion);
            ejecutor1.ExecuteNonQuery();
            conexion.Close();
            MessageBox.Show("Sus datos se eliminaron satisfactoriamente.");
        }
    }
}
