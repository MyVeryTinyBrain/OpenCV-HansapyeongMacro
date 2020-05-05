namespace HansapyeongMacroOpenCV
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_start = new System.Windows.Forms.Button();
            this.textbox_logpanel = new System.Windows.Forms.RichTextBox();
            this.comboBox_startState = new System.Windows.Forms.ComboBox();
            this.button_option = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(12, 380);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(210, 58);
            this.button_start.TabIndex = 1;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // textbox_logpanel
            // 
            this.textbox_logpanel.Location = new System.Drawing.Point(14, 12);
            this.textbox_logpanel.Name = "textbox_logpanel";
            this.textbox_logpanel.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textbox_logpanel.Size = new System.Drawing.Size(210, 362);
            this.textbox_logpanel.TabIndex = 3;
            this.textbox_logpanel.Text = "";
            // 
            // comboBox_startState
            // 
            this.comboBox_startState.FormattingEnabled = true;
            this.comboBox_startState.Location = new System.Drawing.Point(14, 444);
            this.comboBox_startState.Name = "comboBox_startState";
            this.comboBox_startState.Size = new System.Drawing.Size(177, 20);
            this.comboBox_startState.TabIndex = 4;
            // 
            // button_option
            // 
            this.button_option.Font = new System.Drawing.Font("굴림", 9F);
            this.button_option.Location = new System.Drawing.Point(197, 444);
            this.button_option.Name = "button_option";
            this.button_option.Size = new System.Drawing.Size(25, 20);
            this.button_option.TabIndex = 5;
            this.button_option.Text = "...";
            this.button_option.UseVisualStyleBackColor = true;
            this.button_option.Click += new System.EventHandler(this.button_option_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(236, 473);
            this.Controls.Add(this.button_option);
            this.Controls.Add(this.comboBox_startState);
            this.Controls.Add(this.textbox_logpanel);
            this.Controls.Add(this.button_start);
            this.Name = "Form1";
            this.Text = "HansapyeongMacro";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.RichTextBox textbox_logpanel;
        private System.Windows.Forms.ComboBox comboBox_startState;
        private System.Windows.Forms.Button button_option;
    }
}

