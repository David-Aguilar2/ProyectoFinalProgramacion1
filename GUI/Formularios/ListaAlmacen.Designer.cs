namespace GUI
{
    partial class ListaAlmacen
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
        private System.Windows.Forms.DataGridView dgvAlmacen;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaAlmacen));
            this.dgvAlmacen = new System.Windows.Forms.DataGridView();
            this.RMPrincipal = new System.Windows.Forms.Button();
            this.agregar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gCategorias = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBuscarId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlmacen)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAlmacen
            // 
            this.dgvAlmacen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAlmacen.BackgroundColor = System.Drawing.Color.Orchid;
            this.dgvAlmacen.ColumnHeadersHeight = 29;
            this.dgvAlmacen.GridColor = System.Drawing.Color.DarkMagenta;
            this.dgvAlmacen.Location = new System.Drawing.Point(26, 212);
            this.dgvAlmacen.Name = "dgvAlmacen";
            this.dgvAlmacen.ReadOnly = true;
            this.dgvAlmacen.RowHeadersWidth = 51;
            this.dgvAlmacen.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAlmacen.Size = new System.Drawing.Size(1190, 265);
            this.dgvAlmacen.TabIndex = 15;
            this.dgvAlmacen.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAlmacen_CellContentClick);
            // 
            // RMPrincipal
            // 
            this.RMPrincipal.BackColor = System.Drawing.Color.Thistle;
            this.RMPrincipal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RMPrincipal.Location = new System.Drawing.Point(40, 86);
            this.RMPrincipal.Name = "RMPrincipal";
            this.RMPrincipal.Size = new System.Drawing.Size(129, 55);
            this.RMPrincipal.TabIndex = 21;
            this.RMPrincipal.Text = "Regresar Menú Principal";
            this.RMPrincipal.UseVisualStyleBackColor = false;
            this.RMPrincipal.Click += new System.EventHandler(this.RMPrincipal_Click);
            // 
            // agregar
            // 
            this.agregar.BackColor = System.Drawing.Color.Thistle;
            this.agregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agregar.Location = new System.Drawing.Point(1069, 86);
            this.agregar.Name = "agregar";
            this.agregar.Size = new System.Drawing.Size(129, 55);
            this.agregar.TabIndex = 24;
            this.agregar.Text = "Agregar producto";
            this.agregar.UseVisualStyleBackColor = false;
            this.agregar.Click += new System.EventHandler(this.agregar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(458, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 38);
            this.label1.TabIndex = 23;
            this.label1.Text = "Lista de productos";
            // 
            // gCategorias
            // 
            this.gCategorias.BackColor = System.Drawing.Color.Thistle;
            this.gCategorias.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gCategorias.Location = new System.Drawing.Point(539, 86);
            this.gCategorias.Name = "gCategorias";
            this.gCategorias.Size = new System.Drawing.Size(129, 55);
            this.gCategorias.TabIndex = 25;
            this.gCategorias.Text = "Gestionar categorias";
            this.gCategorias.UseVisualStyleBackColor = false;
            this.gCategorias.Click += new System.EventHandler(this.gCategorias_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 18);
            this.label2.TabIndex = 26;
            this.label2.Text = "Buscar por ID:";
            // 
            // txtBuscarId
            // 
            this.txtBuscarId.Location = new System.Drawing.Point(358, 173);
            this.txtBuscarId.Name = "txtBuscarId";
            this.txtBuscarId.Size = new System.Drawing.Size(151, 24);
            this.txtBuscarId.TabIndex = 27;
            this.txtBuscarId.TextChanged += new System.EventHandler(this.txtBuscarId_TextChanged);
            // 
            // ListaAlmacen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(1238, 499);
            this.Controls.Add(this.txtBuscarId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gCategorias);
            this.Controls.Add(this.agregar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RMPrincipal);
            this.Controls.Add(this.dgvAlmacen);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ListaAlmacen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IconicFashion | Gestión del Almacen";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlmacen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button RMPrincipal;
        private System.Windows.Forms.Button agregar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button gCategorias;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBuscarId;
    }
}