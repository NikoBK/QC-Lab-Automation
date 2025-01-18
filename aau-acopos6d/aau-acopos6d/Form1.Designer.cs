namespace aau_acopos6d
{
    partial class Form1
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
            this.send_bot_to_queue = new System.Windows.Forms.Button();
            this.send_queue_to_highway = new System.Windows.Forms.Button();
            this.input = new System.Windows.Forms.TextBox();
            this.reset_highway_test = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.queue_test = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // send_bot_to_queue
            // 
            this.send_bot_to_queue.Location = new System.Drawing.Point(350, 343);
            this.send_bot_to_queue.Name = "send_bot_to_queue";
            this.send_bot_to_queue.Size = new System.Drawing.Size(193, 37);
            this.send_bot_to_queue.TabIndex = 11;
            this.send_bot_to_queue.Text = "Add to queue";
            this.send_bot_to_queue.UseVisualStyleBackColor = true;
            this.send_bot_to_queue.Click += new System.EventHandler(this.send_bot_to_queue_Click);
            // 
            // send_queue_to_highway
            // 
            this.send_queue_to_highway.Location = new System.Drawing.Point(12, 430);
            this.send_queue_to_highway.Name = "send_queue_to_highway";
            this.send_queue_to_highway.Size = new System.Drawing.Size(122, 81);
            this.send_queue_to_highway.TabIndex = 12;
            this.send_queue_to_highway.Text = "Start";
            this.send_queue_to_highway.UseVisualStyleBackColor = true;
            this.send_queue_to_highway.Click += new System.EventHandler(this.send_queue_to_highway_Click);
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(12, 346);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(332, 31);
            this.input.TabIndex = 13;
            // 
            // reset_highway_test
            // 
            this.reset_highway_test.Location = new System.Drawing.Point(18, 151);
            this.reset_highway_test.Margin = new System.Windows.Forms.Padding(9);
            this.reset_highway_test.Name = "reset_highway_test";
            this.reset_highway_test.Size = new System.Drawing.Size(176, 116);
            this.reset_highway_test.TabIndex = 10;
            this.reset_highway_test.Text = "Return highway test to start position";
            this.reset_highway_test.UseVisualStyleBackColor = true;
            this.reset_highway_test.Click += new System.EventHandler(this.reset_highway_test_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 18);
            this.button1.Margin = new System.Windows.Forms.Padding(9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(176, 115);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start up";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 318);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(308, 25);
            this.label1.TabIndex = 14;
            this.label1.Text = "Insert string of stations in order";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(350, 244);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(193, 56);
            this.button2.TabIndex = 15;
            this.button2.Text = "Reset input rack";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // queue_test
            // 
            this.queue_test.Location = new System.Drawing.Point(350, 170);
            this.queue_test.Name = "queue_test";
            this.queue_test.Size = new System.Drawing.Size(193, 56);
            this.queue_test.TabIndex = 16;
            this.queue_test.Text = "Queue test";
            this.queue_test.UseVisualStyleBackColor = true;
            this.queue_test.Click += new System.EventHandler(this.queue_test_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 523);
            this.Controls.Add(this.queue_test);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reset_highway_test);
            this.Controls.Add(this.input);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.send_queue_to_highway);
            this.Controls.Add(this.send_bot_to_queue);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button send_bot_to_queue;
        private System.Windows.Forms.Button send_queue_to_highway;
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.Button reset_highway_test;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button queue_test;
    }
}
