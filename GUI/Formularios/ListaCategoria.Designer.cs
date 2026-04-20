namespace GUI.Formularios
{
    partial class ListaCategoria
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crud = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.RMPrincipal = new System.Windows.Forms.Button();
            this.dgvAlmacen = new System.Windows.Forms.DataGridView();
            this.RGproductos = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlmacen)).BeginInit();
            this.SuspendLayout();
            // 
            // crud
            // 
            this.crud.BackColor = System.Drawing.Color.Thistle;
            this.crud.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crud.Location = new System.Drawing.Point(1067, 86);
            this.crud.Name = "crud";
            this.crud.Size = new System.Drawing.Size(129, 55);
            this.crud.TabIndex = 28;
            this.crud.Text = "Administrar categorías";
            this.crud.UseVisualStyleBackColor = false;
            this.crud.Click += new System.EventHandler(this.crud_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(456, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 38);
            this.label1.TabIndex = 27;
            this.label1.Text = "Lista de categorías";
            // 
            // RMPrincipal
            // 
            this.RMPrincipal.BackColor = System.Drawing.Color.Thistle;
            this.RMPrincipal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RMPrincipal.Location = new System.Drawing.Point(38, 86);
            this.RMPrincipal.Name = "RMPrincipal";
            this.RMPrincipal.Size = new System.Drawing.Size(129, 55);
            this.RMPrincipal.TabIndex = 26;
            this.RMPrincipal.Text = "Regresar Menú Principal";
            this.RMPrincipal.UseVisualStyleBackColor = false;
            // 
            // dgvAlmacen
            // 
            this.dgvAlmacen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAlmacen.BackgroundColor = System.Drawing.Color.Orchid;
            this.dgvAlmacen.ColumnHeadersHeight = 29;
            this.dgvAlmacen.GridColor = System.Drawing.Color.DarkMagenta;
            this.dgvAlmacen.Location = new System.Drawing.Point(24, 160);
            this.dgvAlmacen.Name = "dgvAlmacen";
            this.dgvAlmacen.ReadOnly = true;
            this.dgvAlmacen.RowHeadersWidth = 51;
            this.dgvAlmacen.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAlmacen.Size = new System.Drawing.Size(1190, 265);
            this.dgvAlmacen.TabIndex = 25;
            // 
            // RGproductos
            // 
            this.RGproductos.BackColor = System.Drawing.Color.Thistle;
            this.RGproductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RGproductos.Location = new System.Drawing.Point(503, 79);
            this.RGproductos.Name = "RGproductos";
            this.RGproductos.Size = new System.Drawing.Size(194, 68);
            this.RGproductos.TabIndex = 29;
            this.RGproductos.Text = "Regresar a gestión productos";
            this.RGproductos.UseVisualStyleBackColor = false;
            this.RGproductos.Click += new System.EventHandler(this.RGproductos_Click);
            // 
            // ListaCategoria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 447);
            this.Controls.Add(this.RGproductos);
            this.Controls.Add(this.crud);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RMPrincipal);
            this.Controls.Add(this.dgvAlmacen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ListaCategoria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IconicFashion | Gestión Categorias";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlmacen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button crud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RMPrincipal;
        private System.Windows.Forms.DataGridView dgvAlmacen;
        private System.Windows.Forms.Button RGproductos;
    }
}