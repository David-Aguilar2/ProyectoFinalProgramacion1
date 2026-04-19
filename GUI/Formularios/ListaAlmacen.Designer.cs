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
            this.dgvAlmacen = new System.Windows.Forms.DataGridView();
            this.RMPrincipal = new System.Windows.Forms.Button();
            this.crud = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
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
            this.dgvAlmacen.Location = new System.Drawing.Point(26, 160);
            this.dgvAlmacen.Name = "dgvAlmacen";
            this.dgvAlmacen.ReadOnly = true;
            this.dgvAlmacen.RowHeadersWidth = 51;
            this.dgvAlmacen.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAlmacen.Size = new System.Drawing.Size(1190, 265);
            this.dgvAlmacen.TabIndex = 15;
            this.dgvAlmacen.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAlmacen_CellClick);
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
            // crud
            // 
            this.crud.BackColor = System.Drawing.Color.Thistle;
            this.crud.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crud.Location = new System.Drawing.Point(1069, 86);
            this.crud.Name = "crud";
            this.crud.Size = new System.Drawing.Size(129, 55);
            this.crud.TabIndex = 24;
            this.crud.Text = "Administrar productos";
            this.crud.UseVisualStyleBackColor = false;
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
            // ListaAlmacen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(1238, 447);
            this.Controls.Add(this.crud);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RMPrincipal);
            this.Controls.Add(this.dgvAlmacen);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ListaAlmacen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión del Almacen";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlmacen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button RMPrincipal;
        private System.Windows.Forms.Button crud;
        private System.Windows.Forms.Label label1;
    }
}