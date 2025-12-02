using System;
using MAD.DAO;
using MAD.Entidad;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ImageDrawing = System.Drawing.Image;
using ImagePDF = iTextSharp.text.Image;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static MAD.Entidad.InicioSesion;

namespace MAD
{
    public partial class Form6: Form
    {

        //NominaDAO dao = new NominaDAO();

        public Form6()
        {
            InitializeComponent();
        }

        string rol = SesionActual.Rol;


        private void Form6_Load(object sender, EventArgs e)
        {
            NominaDAO.LlenarComboDepartamentos(comboBox1);
            //dataGridView1.DataSource = NominaDAO.ObtenerDetalleNomina();
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private int MesANumero(string mes)
        {
            switch (mes)
            {
                case "Enero": return 1;
                case "Febrero": return 2;
                case "Marzo": return 3;
                case "Abril": return 4;
                case "Mayo": return 5;
                case "Junio": return 6;
                case "Julio": return 7;
                case "Agosto": return 8;
                case "Septiembre": return 9;
                case "Octubre": return 10;
                case "Noviembre": return 11;
                case "Diciembre": return 12;
                default: return 0;
            }
        }

        /*
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            int idEmpleado = int.Parse(textBoxID.Text);
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);


            ResultadoNomina nom = dao.CalcularNomina(idEmpleado, mes, anio);


            lblPercepciones.Text = nom.TotalPercepciones.ToString("C");
            lblDeducciones.Text = nom.TotalDeducciones.ToString("C");
            lblPagoFinal.Text = nom.PagoFinal.ToString("C");


            //var tabla = new List<dynamic>();


            //foreach (var p in nom.ListaPercepciones)
            //    tabla.Add(new { Concepto = p });


            //foreach (var d in nom.ListaDeducciones)
            //    tabla.Add(new { Concepto = d });


            //dataGridView1.DataSource = tabla;
        }*/

        /*private void btnCalcular_Click(object sender, EventArgs e)
        {
            string nombreDepartamento = comboBox1.Text;

            MessageBox.Show("Mes seleccionado: " + comboMes.Text);
            MessageBox.Show("Año seleccionado: " + comboAnio.Text);


            int mes = MesANumero(comboMes.Text.Trim());
            int anio = int.Parse(comboAnio.Text);

            // Obtener todos los empleados filtrados
            var lista = NominaDAO.ObtenerEmpleadosFiltrados(
                nombreDepartamento, mes, anio);

            if (lista.Count == 0)
            {
                MessageBox.Show("No se encontraron empleados para ese departamento, mes y año.");
                return;
            }

            // Si quieres solo ver cuántos coincidieron:
            MessageBox.Show("Empleados encontrados: " + lista.Count);

            // Si quieres recorrerlos
            foreach (var emp in lista)
            {
                // Aquí puedes hacer cálculos por empleado
                // ej:
                Console.WriteLine($"Empleado: {emp.idEmpl}, Salario: {emp.SalDr}");
            }

            // Si quieres guardar la lista en una variable global para usar en otro form:
            empleadosFiltrados = lista;  // <- si lo necesitas, lo implementamos también
        }*/

        /*private void btnCalcular_Click(object sender, EventArgs e)
        {
            string nombreDepartamento = comboBox1.Text;
            //string empresa = comboEmpresa.Text;   // si tienes el combo
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);

            var empleados = NominaDAO.ObtenerEmpleadosFiltrados(nombreDepartamento);

            foreach (var emp in empleados)
            {
                var movimientos = NominaDAO.ObtenerMovimientosEmpleado(emp.idEmpl, emp.Empl_Dep, mes, anio);

                // -----------------------------
                // 1. Validación de faltas
                // -----------------------------
                bool tieneFalta = movimientos.Any(m =>
                    m.tipoPyD.Trim().Equals("Deducciones", StringComparison.OrdinalIgnoreCase) &&
                    m.concepto.Trim().Equals("Faltas", StringComparison.OrdinalIgnoreCase)
                );

                // DECLARAR diasTrabajados AQUÍ para que esté disponible en todo el foreach
                int diasTrabajados = tieneFalta ? 29 : 30;
                int incidencias = tieneFalta ? 1 : 0;

                // -----------------------------
                // 2. Salarios
                // -----------------------------
                decimal salarioMensual = emp.SalDr * diasTrabajados;
                decimal salarioIntegrado = emp.SalDr * 1.0493m;

                // -----------------------------
                // 3. Percepciones
                // -----------------------------
                decimal despensa = salarioMensual * 0.13m;
                decimal bonoProductividad = salarioMensual * 0.18m;
                decimal aguinaldo = emp.SalDr * 18m;

                decimal montoPuntualidad = movimientos
                    .Where(m => m.concepto == "Bono puntualidad")
                    .Select(m => m.monto).FirstOrDefault();
                decimal bonoPuntualidad = (montoPuntualidad / 30m) * diasTrabajados;

                decimal montoAsistencia = movimientos
                    .Where(m => m.concepto == "Bono asistencia")
                    .Select(m => m.monto).FirstOrDefault();
                decimal bonoAsistencia = (montoAsistencia / 30m) * diasTrabajados;

                // Total percepciones
                decimal totalPercepciones = salarioMensual + despensa + bonoProductividad +
                                            aguinaldo + bonoPuntualidad + bonoAsistencia;

                // -----------------------------
                // 4. Deducciones
                // -----------------------------
                decimal imss = (salarioIntegrado * diasTrabajados) * 0.028m;
                decimal isr = salarioMensual * 0.12m;
                decimal seguroSocial = salarioMensual * 0.02m;
                decimal impuestoRenta = salarioMensual * 0.03m;

                decimal totalDeducciones = imss + isr + seguroSocial + impuestoRenta;

                // -----------------------------
                // 5. Salarios finales
                // -----------------------------
                decimal salarioBruto = salarioMensual;
                decimal salarioTotal = totalPercepciones - totalDeducciones;

                // -----------------------------
                // 6. GUARDAR EN LA BASE
                // -----------------------------
                NominaDAO.InsertarDetalleNomina(
                    emp.idEmpl,
                    mes,
                    anio,
                    emp.Empl_Emp,
                    emp.Empl_Dep,
                    salarioMensual,
                    salarioIntegrado,
                    diasTrabajados,
                    incidencias,
                    salarioBruto,
                    totalPercepciones,
                    totalDeducciones,
                    salarioTotal
                );
            }

            MessageBox.Show("Nómina generada y guardada correctamente.");
        }*/


        private void btnCalcular_Click(object sender, EventArgs e)
        {
            string nombreDepartamento = comboBox1.Text;
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);

            var empleados = NominaDAO.ObtenerEmpleadosFiltrados(nombreDepartamento);

            foreach (var emp in empleados)
            {
                // Movimientos de tabla Perc_Dedc (departamento + empleado)
                var movimientos = NominaDAO.ObtenerMovimientosEmpleado(
                    emp.idEmpl,
                    emp.Empl_Dep,
                    mes,
                    anio
                );


                // ----------- 1. Revisar faltas ------------
                // Contar cuántas faltas hay realmente
                int faltas = movimientos.Count(m =>
                    m.tipoPyD == "Deducciones" &&
                    m.concepto.Trim().Equals("Faltas", StringComparison.OrdinalIgnoreCase)
                );

                // Días trabajados = 30 - faltas
                int diasTrabajados = 30 - faltas;
                if (diasTrabajados < 0) diasTrabajados = 0; // seguridad

                // El número real de incidencias
                int incidencias = faltas;

                // ----------- 2. Sueldos base (formulas fijas) ------------
                decimal salarioMensual = emp.SalDr * diasTrabajados;
                decimal salarioIntegrado = emp.SalDr * 1.0493m;

                // ----------- 3. Percepciones desde SQL ------------
                decimal totalPercepcionesSQL = movimientos
                    .Where(m => m.tipoPyD == "Percepciones")
                    .Sum(m => m.monto);

                decimal BaseBonos = emp.SalDr * 30;
                // ----------- (Opcional) Percepciones fijas -----------

                // Si quieres mantener alguna fórmula fija SOLO SI está en la tabla:
                decimal BonoPunt = 0;
                if (movimientos.Any(m => m.concepto == "Bono puntualidad"))
                {
                    BonoPunt = BaseBonos * 0.09m;

                    if (diasTrabajados < 30)
                    {
                        BonoPunt = (BonoPunt / 30) * diasTrabajados;
                    }
                }    

                decimal productividad = 0;
                if (movimientos.Any(m => m.concepto == "Bono productividad"))
                {
                    if (diasTrabajados == 30)
                    {
                        productividad = BaseBonos * 0.07m;
                    }
                }

                decimal BonoAssist = 0;
                if (movimientos.Any(m => m.concepto == "Bono asistencia"))
                {
                    BonoAssist = BaseBonos * 0.08m;

                    if (diasTrabajados < 30)
                    {
                        BonoAssist = (BaseBonos/30)*diasTrabajados;
                    }
                }

                decimal despensa = 0;
                if (movimientos.Any(m => m.concepto == "Despensa"))
                    despensa = BaseBonos * 0.12m;

                // Total percepciones = SQL + fórmulas activadas
                decimal totalPercepciones =
                      //totalPercepcionesSQL +
                      despensa
                    + BonoPunt
                    + productividad
                    + BonoAssist;

                // ----------- 4. Deducciones desde SQL ------------
                decimal totalDeduccionesSQL = movimientos
                    .Where(m => m.tipoPyD == "Deducciones")
                    .Sum(m => m.monto);

                // ----------- (Opcional) Deducciones fijas -----------

                decimal salarioMinimo = 278.80m;

                decimal imss = 0;
                if (movimientos.Any(m => m.concepto == "IMSS"))
                    imss = (salarioIntegrado * diasTrabajados) * 0.028m;

                decimal isrFormula = 0;
                if (movimientos.Any(m => m.concepto == "ISR"))
                    //isrFormula = salarioMensual * 0.12m;
                    isrFormula = NominaDAO.CalcularISRReal(salarioMensual);

                if (emp.SalDr <= salarioMinimo)
                {
                    imss = 0;
                    isrFormula = 0;
                }

                    // Total deducciones
                    decimal totalDeducciones =
                      totalDeduccionesSQL +
                      imss
                    + isrFormula;

                // ----------- 5. Totales finales ------------
                decimal salarioBruto = salarioMensual + totalPercepciones; ;
                decimal salarioNeto = salarioBruto - totalDeducciones;

                Console.WriteLine(
    $"Puntualidad={BonoPunt}, Asistencia={BonoAssist}, Prod={productividad}, " +
    $"Despensa={despensa}, IMSS={imss}, ISR={isrFormula}"
);


                // ----------- 6. Guardar ------------
                NominaDAO.InsertarDetalleNomina(
                    emp.idEmpl,
                    mes,
                    anio,
                    emp.Empl_Emp,
                    emp.Empl_Dep,
                    salarioMensual,
                    salarioIntegrado,
                    diasTrabajados,
                    incidencias,
                    salarioBruto,
                    BonoPunt,
                    BonoAssist,
                    productividad,
                    despensa,
                    totalPercepciones,
                    imss,
                    isrFormula,
                    totalDeducciones,
                    salarioNeto
                );


            }

            MessageBox.Show("Nómina generada y guardada correctamente.");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);

            bool guardado = NominaDAO.RegistrarMesProcesado(mes, anio);

            if (guardado)
                MessageBox.Show("Mes y año registrados correctamente.");
            else
                MessageBox.Show("Este mes ya fue registrado previamente.");
        }

        /*private void btnGenerarCSV_Click(object sender, EventArgs e)
        {
            try
            {
                int mes = MesANumero(comboMes.Text);
                int anio = int.Parse(comboAnio.Text);
                int departamento = int.Parse(comboBox1.SelectedValue.ToString());

                DataTable dt = NominaDAO.ObtenerDetalleNomina(mes, anio, departamento);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para exportar.");
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV (*.csv)|*.csv";
                saveDialog.FileName = $"Nomina_{mes}_{anio}_{departamento}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    GenerarCSV(dt, saveDialog.FileName);
                    MessageBox.Show("CSV generado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar CSV: " + ex.Message);
            }
        }

        public void GenerarCSV(DataTable dt, string ruta)
        {
            StringBuilder sb = new StringBuilder();

            // Encabezados
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append(dt.Columns[i].ColumnName);
                if (i < dt.Columns.Count - 1) sb.Append(",");
            }
            sb.AppendLine();

            // Filas
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(row[i].ToString());
                    if (i < dt.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();
            }

            File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
        }*/


        //Menu

        private void empleadosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Reg_Empl f1 = new Reg_Empl();
            f1.Show();
            this.Hide();
        }

        private void departamentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rol == "Administrador")
            {
                Form2 f2 = new Form2();
                f2.Show();
                this.Hide();
            }
        }

        private void puestosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rol == "Administrador")
            {
                Form3 f3 = new Form3();
                f3.Show();
                this.Hide();
            }
        }
        private void empresasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rol == "Administrador")
            {
                Form4 f4 = new Form4();
                f4.Show();
                this.Hide();
            }
        }

        private void PercepAndDeducToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
            this.Hide();
        }

        private void CalcularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form6 f6 = new Form6();
            //f6.Show();
            //this.Hide();
        }

        private void generalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.Show();
            this.Hide();
        }

        private void headcounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.Show();
            this.Hide();
        }

        private void detalladaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.Show();
            this.Hide();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);
            int idDep = Convert.ToInt32(comboBox1.SelectedValue);

            dataGridView1.DataSource = NominaDAO.ObtenerDetalleNominaFiltrado(mes, anio, idDep);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btnGenerarCSV_Click(object sender, EventArgs e)
        {
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);
            int idDep = Convert.ToInt32(comboBox1.SelectedValue);

            // Obtener datos filtrados
            DataTable tabla = NominaDAO.ObtenerDetalleNominaParaCSV(mes, anio, idDep);

            if (tabla.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros con esos filtros.");
                return;
            }

            // Carpeta en escritorio
            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CSV_Nomina";
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            // Nombre del archivo
            string archivo = $"{carpeta}\\Nomina_{mes}_{anio}_Depto_{idDep}.csv";

            using (StreamWriter writer = new StreamWriter(archivo))
            {
                // ENCABEZADOS
                for (int i = 0; i < tabla.Columns.Count; i++)
                {
                    writer.Write(tabla.Columns[i].ColumnName);
                    if (i < tabla.Columns.Count - 1) writer.Write(",");
                }
                writer.WriteLine();

                // FILAS
                foreach (DataRow fila in tabla.Rows)
                {
                    for (int i = 0; i < tabla.Columns.Count; i++)
                    {
                        string valor = fila[i].ToString().Replace(",", " "); // evita romper columnas
                        writer.Write(valor);
                        if (i < tabla.Columns.Count - 1) writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }

            MessageBox.Show("Archivo CSV creado en el escritorio en la carpeta CSV_Nomina.");
        }

        /*private void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);
            int idDep = Convert.ToInt32(comboBox1.SelectedValue);

            DataTable tabla = NominaDAO.ObtenerDetalleNominaParaPDF(mes, anio, idDep);

            if (tabla.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros para generar PDF.");
                return;
            }

            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\PDF_Nomina";
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            // UN SOLO PDF CON TODO (como pediste)
            string archivoPDF = $"{carpeta}\\Nomina_{mes}_{anio}_Depto_{idDep}.pdf";

            Document doc = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            PdfWriter.GetInstance(doc, new FileStream(archivoPDF, FileMode.Create));

            doc.Open();

            // TÍTULO
            Paragraph titulo = new Paragraph($"Reporte de Nómina\nMes: {mes}  Año: {anio}\n\n",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18));
            titulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(titulo);

            // TABLA PDF
            PdfPTable pdfTable = new PdfPTable(tabla.Columns.Count);
            pdfTable.WidthPercentage = 100;

            // ENCABEZADOS
            foreach (DataColumn col in tabla.Columns)
            {
                PdfPCell celda = new PdfPCell(new Phrase(col.ColumnName,
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                celda.BackgroundColor = BaseColor.LIGHT_GRAY;
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(celda);
            }

            // FILAS
            foreach (DataRow row in tabla.Rows)
            {
                foreach (var value in row.ItemArray)
                {
                    pdfTable.AddCell(new Phrase(value.ToString(),
                        FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                }
            }

            doc.Add(pdfTable);
            doc.Close();

            MessageBox.Show("PDF generado correctamente en el escritorio.");
        }*/

        /*private void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);
            int idDep = Convert.ToInt32(comboBox1.SelectedValue);

            DataTable tabla = NominaDAO.ObtenerDetalleNominaParaPDF(mes, anio, idDep);

            if (tabla.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros para generar PDF.");
                return;
            }

            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\PDF_Nomina";
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string archivoPDF = $"{carpeta}\\Nomina_{mes}_{anio}_Depto_{idDep}.pdf";

            Document doc = new Document(PageSize.A4.Rotate(), 30, 30, 40, 30);
            PdfWriter.GetInstance(doc, new FileStream(archivoPDF, FileMode.Create));

            doc.Open();

            // -------------------------
            // ENCABEZADO BONITO
            // -------------------------
            PdfPTable header = new PdfPTable(2);
            header.WidthPercentage = 100;
            header.SetWidths(new float[] { 1f, 3f });

            // LOGO (opcional)
            try
            {
                string rutaLogo = "C:\\Users\\olive\\Downloads\\Logo_Fcfm.jpg";
                if (File.Exists(rutaLogo))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                    logo.ScaleAbsolute(80, 80);

                    PdfPCell celdaLogo = new PdfPCell(logo);
                    celdaLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    celdaLogo.HorizontalAlignment = Element.ALIGN_LEFT;
                    header.AddCell(celdaLogo);
                }
                else
                {
                    PdfPCell celdaVacia = new PdfPCell(new Phrase(""));
                    celdaVacia.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    header.AddCell(celdaVacia);
                }
            }
            catch
            {
                header.AddCell(new PdfPCell(new Phrase(" "))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER
                });
            }

            // TEXTO DEL ENCABEZADO
            PdfPCell titulo = new PdfPCell(new Phrase(
                $"REPORTE DE NÓMINA\nDepartamento: {comboBox1.Text}\nMes: {mes}   Año: {anio}",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)
            ));

            titulo.Border = iTextSharp.text.Rectangle.NO_BORDER;
            titulo.HorizontalAlignment = Element.ALIGN_CENTER;
            titulo.VerticalAlignment = Element.ALIGN_MIDDLE;
            header.AddCell(titulo);

            doc.Add(header);
            doc.Add(new Paragraph("\n")); // Espacio

            // -------------------------
            // TABLA
            // -------------------------
            PdfPTable pdfTable = new PdfPTable(tabla.Columns.Count);
            pdfTable.WidthPercentage = 100;

            BaseColor azul = new BaseColor(41, 128, 185);
            BaseColor grisClaro = new BaseColor(240, 240, 240);

            // ENCABEZADOS
            foreach (DataColumn col in tabla.Columns)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(
                    col.ColumnName,
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE)
                ));

                headerCell.BackgroundColor = azul;
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell.Padding = 6;
                pdfTable.AddCell(headerCell);
            }

            // FILAS ZEBRA
            bool gris = false;

            foreach (DataRow row in tabla.Rows)
            {
                foreach (var value in row.ItemArray)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(
                        value.ToString(),
                        FontFactory.GetFont(FontFactory.HELVETICA, 9)
                    ));

                    cell.Padding = 4;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;

                    cell.BackgroundColor = gris ? grisClaro : BaseColor.WHITE;

                    pdfTable.AddCell(cell);
                }
                gris = !gris;
            }

            doc.Add(pdfTable);

            // Pie de página
            doc.Add(new Paragraph($"\nGenerado el {DateTime.Now}",
                FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8)));

            doc.Close();

            MessageBox.Show("PDF bonito generado correctamente 😎📄");
        }*/

        private void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            int mes = MesANumero(comboMes.Text);
            int anio = int.Parse(comboAnio.Text);
            int idDep = Convert.ToInt32(comboBox1.SelectedValue);

            // Obtenemos la tabla desde tu DAO (1 fila por empleado)
            DataTable tabla = NominaDAO.ObtenerDetalleNominaParaPDF(mes, anio, idDep);

            if (tabla.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros para este filtro.");
                return;
            }

            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RecibosNomina";
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            foreach (DataRow row in tabla.Rows)
            {
                GenerarReciboIndividual(row, carpeta);
            }

            MessageBox.Show("Recibos generados correctamente");
        }

        private void GenerarReciboIndividual(DataRow data, string carpeta)
        {
            string nombreEmpleado = data["NombreEmpleado"].ToString();
            string archivo = $"{carpeta}\\Recibo_{nombreEmpleado.Replace(" ", "_")}.pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
            doc.Open();

            // ---------------- ENCABEZADO ----------------
            PdfPTable header = new PdfPTable(1);
            header.WidthPercentage = 100;

            PdfPCell titulo = new PdfPCell(new Phrase("RECIBO DE NÓMINA",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = iTextSharp.text.Rectangle.NO_BORDER,
                PaddingBottom = 10
            };
            header.AddCell(titulo);

            PdfPCell empresa = new PdfPCell(new Phrase(
                "Empresa: " + data["Empresa"].ToString(),
                FontFactory.GetFont(FontFactory.HELVETICA, 12)))
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            header.AddCell(empresa);

            doc.Add(header);
            doc.Add(new Paragraph("\n"));

            // ---------------- DATOS GENERALES ----------------
            PdfPTable info = new PdfPTable(2);
            info.WidthPercentage = 100;
            info.SetWidths(new float[] { 1f, 2f });

            AgregarFila(info, "Empleado:", data["NombreEmpleado"].ToString());
            AgregarFila(info, "Departamento:", data["Departamento"].ToString());
            AgregarFila(info, "Mes:", data["mes"].ToString());
            AgregarFila(info, "Año:", data["anio"].ToString());
            AgregarFila(info, "Días trabajados:", data["DTrabj"].ToString());
            AgregarFila(info, "Incidencias:", data["incid"].ToString());

            doc.Add(info);
            doc.Add(new Paragraph("\n"));

            // ---------------- PERCEPCIONES ----------------
            PdfPTable percp = new PdfPTable(2);
            percp.WidthPercentage = 100;
            percp.SetWidths(new float[] { 2f, 1f });

            AgregarTituloSeccion(percp, "PERCEPCIONES");
            AgregarFila(percp, "Salario mensual:", data["SalMen"].ToString());
            AgregarFila(percp, "Salario integrado:", data["SalInteg"].ToString());
            AgregarFila(percp, "Bono puntualidad:", data["BonoPunt"].ToString());
            AgregarFila(percp, "Bono asistencia:", data["BonoAssist"].ToString());
            AgregarFila(percp, "Bono productividad:", data["BonoProd"].ToString());
            AgregarFila(percp, "Despensa:", data["Despensa"].ToString());
            AgregarFila(percp, "Total percepciones:", data["Percp"].ToString(), true);

            doc.Add(percp);
            doc.Add(new Paragraph("\n"));

            // ---------------- DEDUCCIONES ----------------
            PdfPTable ded = new PdfPTable(2);
            ded.WidthPercentage = 100;
            ded.SetWidths(new float[] { 2f, 1f });

            AgregarTituloSeccion(ded, "DEDUCCIONES");
            AgregarFila(ded, "IMSS:", data["IMSS"].ToString());
            AgregarFila(ded, "ISR:", data["ISR"].ToString());
            AgregarFila(ded, "Otras deducciones:", data["Dedc"].ToString());
            AgregarFila(ded, "Total deducciones:", data["Dedc"].ToString(), true);

            doc.Add(ded);
            doc.Add(new Paragraph("\n"));

            // ---------------- TOTAL ----------------
            Paragraph total = new Paragraph(
                "TOTAL A PAGAR: $" + data["SalTotal"].ToString(),
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
            );
            total.Alignment = Element.ALIGN_RIGHT;
            doc.Add(total);

            doc.Add(new Paragraph("\n\n\nFirma del empleado: ___________________________"));

            doc.Close();
        }

        private void AgregarFila(PdfPTable table, string campo, string valor, bool bold = false)
        {
            table.AddCell(new PdfPCell(new Phrase(campo,
                FontFactory.GetFont(FontFactory.HELVETICA, 11)))
            { Border = iTextSharp.text.Rectangle.NO_BORDER });

            table.AddCell(new PdfPCell(new Phrase(
                bold ? valor : valor,
                bold ? FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)
                     : FontFactory.GetFont(FontFactory.HELVETICA, 11)))
            { Border = iTextSharp.text.Rectangle.NO_BORDER });
        }

        private void AgregarTituloSeccion(PdfPTable table, string titulo)
        {
            PdfPCell cell = new PdfPCell(new Phrase(titulo,
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13)))
            {
                BackgroundColor = new BaseColor(220, 220, 220),
                HorizontalAlignment = Element.ALIGN_CENTER,
                Colspan = 2,
                Padding = 5
            };
            table.AddCell(cell);
        }


        //

    }
}
