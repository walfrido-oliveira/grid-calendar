Imports System.Windows.Forms
Imports System.Windows

''' <summary>
''' CONTROLE PERSSONALIZADO PARA CALENDÁRIO
''' DATAGRIDVIEW + 2 BOTOES + 1 LABEL + 1 LINKLABEL
''' </summary>
''' <remarks></remarks>

Public Class GridCalendarControl

    Inherits System.Windows.Forms.UserControl
    Public Delegate Sub DataChangeEventHandler(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs, ByVal data As Date)
    Public Event DataChangeEventArgs As DataChangeEventHandler

#Region "VarDatas"
    Private Day As Integer = Today.Day
    Private Month As Integer = Today.Month
    Private Year As Integer = Today.Year
    Private xDays As Integer
    Private DateV() As Date = {}
    Private DateVIndex As Integer
    Private dataSelecionada As Date = New Date(Today.Year, Today.Month, Today.Day)
#End Region

#Region "VarCalendar"
    Private ColorOfMonth As System.Drawing.Color = System.Drawing.Color.Black
    Private ColorOfBackGround As System.Drawing.Color = System.Drawing.Color.White
    Private ColorOfNextMonth As System.Drawing.Color = System.Drawing.Color.Gray
    Private ColorOfPrevious As System.Drawing.Color = System.Drawing.Color.Gray
    Private ColorOfWeekend As System.Drawing.Color = System.Drawing.Color.LightGray
    Private ColorOfDetachDate As System.Drawing.Color = System.Drawing.Color.Red
    Private blnColor As Boolean
    Private blnNextMonth As Boolean
    Private ColorV() As System.Drawing.Color = {}
    Private InfoV() As String = {}
    Private nextCell As Boolean
    Private previousCell As Boolean
#End Region


    ''' <summary>
    ''' INICIA UMA NOVA INSTÂNCIA DE GridCalendarControl
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        InitializeComponent()
        NewInstance()
    End Sub

    Private Sub NewInstance()
        AddRows()
        CalendarCreate(Me.Day, Me.Month, Me.Year)
        Me.lblData.Text = MothAndYearSelected(Today.Month, Today.Year)
        Me.lklToday.Text = GetToday()
        SelectedToday()
    End Sub

    ''' <summary>
    ''' OBTÉM OU DEFINE DATA ATUAL
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Value() As Date
        Get
            Return Me.dataSelecionada
        End Get
        Set(ByVal value As Date)
            SelectedDay(value)
            Me.dataSelecionada = value
        End Set
    End Property

    ''' <summary>
    ''' DISATIVA SELEÇÃO DE GRID
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableSelected()
        For Each row As DataGridViewRow In Me.dgvCalendario.Rows
            For Each cell As DataGridViewCell In row.Cells
                cell.Selected = False
            Next
        Next
    End Sub

    ''' <summary>
    ''' OBTÉM OU DEFINE COR DE Fundo SELEÇÃO DO COMPONENTE
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ColorSelected() As System.Drawing.Color
        Get
            Return Me.dgvCalendario.DefaultCellStyle.SelectionBackColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            Me.dgvCalendario.DefaultCellStyle.SelectionBackColor = value
        End Set
    End Property

    ''' <summary>
    ''' OBTÉM OU DEFINE A COR DE  DE SELEÇÃO
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ColorForegroundSelected() As System.Drawing.Color
        Get
            Return Me.dgvCalendario.DefaultCellStyle.SelectionForeColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            Me.dgvCalendario.DefaultCellStyle.SelectionForeColor = value
        End Set
    End Property


    '''<summary>
    ''' ADICIONA SE NECESSÁRIO LINHAS QUE IRÃO COMPOR O CALENDÁRIO
    ''' 6X7
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AddRows()
        Dim intRows As Integer = 0
        If dgvCalendario.RowCount = 0 Then
            While intRows < 6
                Me.dgvCalendario.Rows.Add(String.Empty)
                intRows += 1
            End While
        End If
    End Sub

    ''' <summary>
    ''' RETORNA A DATA ATUAL POR EXTENSO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetToday() As String
        Return WeekdayName(Today.DayOfWeek + 1).ToUpper & ", " & Today.Day.ToString & " DE " & MonthName(Today.Month).ToUpper & " DE " & Today.Year.ToString
    End Function

    ''' <summary>
    ''' LIMPA AS CORES MODIFICADOS DO DATAGRIDVIEW
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CleanColor()
        For Each rows As DataGridViewRow In Me.dgvCalendario.Rows
            For Each cells As DataGridViewCell In rows.Cells
                cells.Style.ForeColor = ColorOfMonth
                cells.Style.BackColor = ColorOfBackGround
            Next
        Next
    End Sub

    ''' <summary>
    ''' LIMPA CELULAS SELECIONADAS E SELECIONA O DIA ATUAL
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SelectedToday()
        Me.dgvCalendario.ClearSelection()
        For Each row As DataGridViewRow In Me.dgvCalendario.Rows
            For Each cell As DataGridViewCell In row.Cells
                If CDate(cell.Tag).Equals(Today) Then
                    cell.Selected = True
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' SELECIONA DATA 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <remarks></remarks>
    Private Sub SelectedDay(ByVal data As Date)
        For Each row As DataGridViewRow In Me.dgvCalendario.Rows
            For Each cell As DataGridViewCell In row.Cells
                If CDate(cell.Tag).Equals(data) Then
                    cell.Selected = True
                End If
            Next
        Next
        RaiseEvent DataChangeEventArgs(New Object, New System.Windows.Forms.DataGridViewCellEventArgs(Me.dgvCalendario.CurrentCell.ColumnIndex, Me.dgvCalendario.CurrentCell.RowIndex), _
                                       CatchDate())
    End Sub



    ''' <summary>
    ''' RETORNA O MÊS E O ANO DA DATA SELECIONADA
    ''' </summary>
    ''' <param name="Month"></param>
    ''' <param name="Year"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MothAndYearSelected(ByVal Month As String, ByVal Year As Integer) As String
        Dim strMonthYear As String = String.Empty
        Select Case Month
            Case 1
                strMonthYear = "JANEIRO"
            Case 2
                strMonthYear = "FEVEREIRO"
            Case 3
                strMonthYear = "MARÇO"
            Case 4
                strMonthYear = "ABRIL"
            Case 5
                strMonthYear = "MAIO"
            Case 6
                strMonthYear = "JUNHO"
            Case 7
                strMonthYear = "JULHO"
            Case 8
                strMonthYear = "AGOSTO"
            Case 9
                strMonthYear = "SETEMBRO"
            Case 10
                strMonthYear = "OUTUBRO"
            Case 11
                strMonthYear = "NOVEMBRO"
            Case 12
                strMonthYear = "DEZEMBRO"
        End Select
        strMonthYear += " " & Year.ToString()
        Return strMonthYear
    End Function

    ''' <summary>
    '''  FUNÇÃO PRINCIPAL ONDE É FORMATADO O CALENDÁRIO
    ''' </summary>
    ''' <param name="Day">DIA CORRENTE</param>
    ''' <param name="Month">MES CORRENTE</param>
    ''' <param name="Year">ANO CORRENTE</param>
    ''' <remarks></remarks>
    Private Sub CalendarCreate(ByVal day As Integer, ByVal month As Integer, ByVal year As Integer)
        CleanColor()
        blnColor = False
        Dim tag As Date

        'OBTÉM O DIA DA SEMANA DO PRIMEIRO DO DIA DO MÊS CORRENTE
        Dim DayOfWeek As Integer = CDate("01/" & CStr(month) & "/" & CStr(year)).DayOfWeek

        ' OBTÉM A QUANTIDADE DIAS DO MÊS CORRENTE
        Dim DaysOfMonth As Integer = Date.DaysInMonth(year, month)

        'OBTÉM A QUANTIDADE DIAS DO MÊS ANTERIOR AO CORRENTE
        Dim PreviousMonth As Integer = month - 1
        Dim PreviousYear As Integer = year
        If PreviousMonth < 1 Then
            PreviousMonth = 12
            PreviousYear = year - 1
        End If
        Dim PreviousDaysMonth As Integer = Date.DaysInMonth(PreviousYear, PreviousMonth)

        'OBTÉM A QUANTIDADE DE DIAS DO MÊS SEGUINTE AO O CORRENTE
        Dim NextMonth As Integer = month + 1
        Dim NextYear As Integer = year
        If NextMonth > 12 Then
            NextMonth = 1
            NextYear = year + 1
        End If
        Dim DiasMesProx As Integer = Date.DaysInMonth(NextYear, NextMonth)

        'PREENCHE A CELULA RESPECTIVA COM O PRIMEIRO DIA DO MÊS CORRENTE
        Select Case DayOfWeek
            Case 0
                Me.dgvCalendario.Rows(0).Cells(0).Value = 1
                Me.dgvCalendario.Rows(0).Cells(0).Tag = New Date(year, month, 1)
            Case 1
                Me.dgvCalendario.Rows(0).Cells(1).Value = 1
                Me.dgvCalendario.Rows(0).Cells(1).Tag = New Date(year, month, 1)
            Case 2
                Me.dgvCalendario.Rows(0).Cells(2).Value = 1
                Me.dgvCalendario.Rows(0).Cells(2).Tag = New Date(year, month, 1)
            Case 3
                Me.dgvCalendario.Rows(0).Cells(3).Value = 1
                Me.dgvCalendario.Rows(0).Cells(3).Tag = New Date(year, month, 1)
            Case 4
                Me.dgvCalendario.Rows(0).Cells(4).Value = 1
                Me.dgvCalendario.Rows(0).Cells(4).Tag = New Date(year, month, 1)
            Case 5
                Me.dgvCalendario.Rows(0).Cells(5).Value = 1
                Me.dgvCalendario.Rows(0).Cells(5).Tag = New Date(year, month, 1)
            Case 6
                Me.dgvCalendario.Rows(0).Cells(6).Value = 1
                Me.dgvCalendario.Rows(0).Cells(6).Tag = New Date(year, month, 1)
        End Select

        'PREENCHE O MES ANTERIOR SE NECESSITAR
        xDays = DayOfWeek - 1
        While xDays >= 0
            Me.dgvCalendario.Rows(0).Cells(xDays).Value = PreviousDaysMonth
            If PreviousMonth = 12 Then
                Tag = New Date(PreviousYear, PreviousMonth, PreviousDaysMonth)
            Else
                Tag = New Date(year, PreviousMonth, PreviousDaysMonth)
            End If
            Me.dgvCalendario.Rows(0).Cells(xDays).Tag = Tag
            Me.dgvCalendario.Rows(0).Cells(xDays).Style.ForeColor = ColorOfNextMonth
            PreviousDaysMonth -= 1
            xDays -= 1
        End While

        'PREENCHE OS DIAS DO MÊS CORRENTE
        xDays = 2
        For Each rows As DataGridViewRow In Me.dgvCalendario.Rows
            For Each cells As DataGridViewCell In rows.Cells
                If cells.ColumnIndex = 0 Or cells.ColumnIndex = 6 Then
                    cells.Style.BackColor = ColorOfWeekend
                End If
                If xDays > DaysOfMonth Then
                    xDays = 1
                    blnColor = True
                End If
                If blnColor Then
                    cells.Style.ForeColor = ColorOfNextMonth
                End If
                If cells.ColumnIndex <= DayOfWeek And rows.Index = 0 Then
                    Continue For
                Else
                    If blnColor Then
                        If NextMonth = 1 Then
                            tag = New Date(NextYear, NextMonth, xDays)
                        Else
                            tag = New Date(year, NextMonth, xDays)
                        End If
                    Else
                        tag = New Date(year, month, xDays)
                    End If
                    cells.Value = xDays
                    cells.Tag = tag
                    xDays += 1
                End If
            Next
        Next

        'DISABILITA O REDIMENSIONAMENTO DO DATAGRIDVIEW
        For x As Int32 = 0 To Me.dgvCalendario.ColumnCount - 1
            Me.dgvCalendario.Columns(x).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

    End Sub

    ''' <summary>
    ''' MARCA DATAS SELECIONADAS NO DATAGRID COM FUNDO DE COR DIFERENCIADO
    ''' </summary>
    ''' <param name="dt">DATA A SER MARCADA</param>
    ''' <remarks></remarks>
    Private Sub DetachDate(ByVal dt As Date, ByVal color As System.Drawing.Color)
        For Each row As DataGridViewRow In Me.dgvCalendario.Rows
            For Each cell As DataGridViewCell In row.Cells
                If CDate(cell.Tag).Equals(dt) Then
                    cell.Style.BackColor = color
                End If
            Next
        Next
        Me.dgvCalendario.Update()
        Me.dgvCalendario.Refresh()
    End Sub

    ''' <summary>
    ''' CAPTURA O VETOR DE DATAS E INSERE NO DATAGRIDVIEW O DESTAQUE
    ''' ATRAVES DA FUNÇÃO spiDetachDate
    ''' </summary>
    ''' <param name="Dates">VETOR DE DATAS</param>
    ''' <remarks></remarks>
    Private Sub InsertDate(ByVal Dates As Date(), ByVal color() As System.Drawing.Color)
        Dim x As Integer = 0
        While x < color.Length
            DetachDate(Dates(x), color(x))
            x += 1
        End While
    End Sub


    ''' <summary>
    ''' MARCA DATAS NO CALENDÁRIO
    ''' </summary>
    ''' <param name="Dates">DATAS</param>
    ''' <remarks></remarks>
    Public Sub MarkDate(ByVal Dates() As Date, ByVal Infos() As String, ByVal color As System.Drawing.Color())
        If Not Dates.Length = Infos.Length And Dates.Length = color.Length Then
            MessageBox.Show("Matrizes com tamanhos diferentes. A função será canselada.", "ERRO_MATRIX_IS_NOT_EQUAL", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Me.ColorV = color
        Me.DateV = Dates
        Me.InfoV = Infos
        Dim x As Integer = 0
        While x < color.Length
            DetachDate(Dates(x), color(x))
            x += 1
        End While
    End Sub

    ''' <summary>
    ''' MARCA DATAS NO CALENDÁRIO
    ''' </summary>
    ''' <param name="Dates">DATAS</param>
    ''' <remarks></remarks>
    Public Sub MarkDate(ByVal Dates As Date, ByVal Infos As String, ByVal color As System.Drawing.Color)
        If Me.DateV.Contains(Dates) Then
            Dim index As Integer = FindDateIndex(Me.DateV, Dates)
            Me.ColorV(index) = color
            Me.InfoV(index) += vbCrLf & Infos
        Else
            ReDim Preserve Me.ColorV(Me.ColorV.Length + 1)
            Me.ColorV(Me.ColorV.Length - 1) = color
            ReDim Preserve Me.DateV(Me.DateV.Length + 1)
            Me.DateV(Me.DateV.Length - 1) = Dates
            ReDim Preserve Me.InfoV(Me.InfoV.Length + 1)
            Me.InfoV(Me.InfoV.Length - 1) = Infos
            DetachDate(Dates, color)
        End If
    End Sub

    ''' <summary>
    ''' MARCA DATAS NO CALENDÁRIO
    ''' </summary>
    ''' <param name="Dates">DATAS</param>
    ''' <remarks></remarks>
    Public Sub MarkDate(ByVal Dates As Generic.List(Of Date), ByVal Infos As Generic.List(Of String), ByVal color As Generic.List(Of System.Drawing.Color))
        If Not Dates.Count = Infos.Count And Dates.Count = color.Count Then
            MessageBox.Show("Matrizes com tamanhos diferentes. A função será canselada.", "ERRO_MATRIX_IS_NOT_EQUAL", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Me.ColorV = color.ToArray()
        Me.DateV = Dates.ToArray()
        Me.InfoV = Infos.ToArray()
        Dim x As Integer = 0
        While x < Me.ColorV.Length
            DetachDate(Dates(x), color(x))
            x += 1
        End While
    End Sub

    ''' <summary>
    ''' OBTÉM O INDICE DE UMA DATA EM UM VETOR DE DATAS
    ''' </summary>
    ''' <param name="dates"></param>
    ''' <returns>INDEX</returns>
    ''' <remarks></remarks>
    Private Function FindDateIndex(ByVal dates As Date(), ByVal data As Date) As Integer
        For i As Integer = 0 To dates.Length - 1
            If dates(i) = data Then
                Return i
            End If
        Next
    End Function

    ''' <summary>
    ''' MÊS ANTERIOR
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
        PreviousMonth()
        RaiseEvent DataChangeEventArgs(sender, New System.Windows.Forms.DataGridViewCellEventArgs(Me.dgvCalendario.CurrentCell.ColumnIndex, Me.dgvCalendario.CurrentCell.RowIndex), CatchDate())
    End Sub

    ''' <summary>
    ''' PRÓXIMO MÊS
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        NextMonth()
        RaiseEvent DataChangeEventArgs(sender, New System.Windows.Forms.DataGridViewCellEventArgs(Me.dgvCalendario.CurrentCell.ColumnIndex, Me.dgvCalendario.CurrentCell.RowIndex), CatchDate())
    End Sub

    ''' <summary>
    ''' OBTÉM A DATA ATUAL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lklToday_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lklToday.LinkClicked
        Me.Day = 1
        Me.Month = Today.Month
        Me.Year = Today.Year
        Me.dataSelecionada = New Date(Me.Year, Me.Month, Me.Day)
        Me.lblData.Text = Me.MothAndYearSelected(Today.Month, Today.Year)
        Me.CalendarCreate(Today.Day, Today.Month, Today.Year)
        Me.SelectedToday()
        Me.InsertDate(Me.DateV, Me.ColorV)
        RaiseEvent DataChangeEventArgs(sender, New System.Windows.Forms.DataGridViewCellEventArgs(Me.dgvCalendario.CurrentCell.ColumnIndex, Me.dgvCalendario.CurrentCell.RowIndex), CatchDate())
    End Sub


    ''' <summary>
    ''' MOSTRA TOOLTIP
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvCalendario_CellMouseEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCalendario.CellMouseEnter
        If e.RowIndex > -1 Then
            Dim x As Integer = 0
            Dim text As New Text.StringBuilder()
            For Each Data As Date In Me.DateV
                If Data = CatchDate(e) Then
                    text.Append(Me.InfoV(x) & vbCrLf & vbCrLf)
                End If
                x += 1
            Next
            Me.ToolTip1.ToolTipTitle = "INFORMAÇÕES"
            Me.ToolTip1.Show(text.ToString(), Me.dgvCalendario)
        End If
    End Sub

    ''' <summary>
    ''' FECHA TOOLTIP
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvCalendario_CellMouseLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCalendario.CellMouseLeave
        Me.ToolTip1.Hide(Me.dgvCalendario)
    End Sub

    ''' <summary>
    ''' CAPTURA DATA SELECIONADA
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CatchDate(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Date
        Return CDate(Me.dgvCalendario.Rows(e.RowIndex).Cells(e.ColumnIndex).Tag)
    End Function

    ''' <summary>
    ''' CAPTURA DATA SELECIONADA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CatchDate() As Date
        Return CDate(Me.dgvCalendario.CurrentCell.Tag)
    End Function

    ''' <summary>
    ''' OBTEM DATA SELECIONADA E VAI PRO PROXIMO OU MES ANTERIOR
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvCalendario_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCalendario.CellClick
        Me.dataSelecionada = CDate(Me.dgvCalendario.CurrentCell.Tag)
        RaiseEvent DataChangeEventArgs(sender, e, Me.dataSelecionada)
    End Sub

    ''' <summary>
    ''' TECLA NO DATAGRIDVIEW
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvCalendario_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvCalendario.KeyUp
        If dgvCalendario.CurrentCell.RowIndex = 0 Then
            If CInt(dgvCalendario.CurrentCell.Value) > 7 Then
                PreviousMonth()
            End If
        ElseIf dgvCalendario.CurrentCell.RowIndex >= 4 Then
            If CInt(dgvCalendario.CurrentCell.Value) <= 14 Then
                NextMonth()
            End If
        End If
    End Sub

    ''' <summary>
    ''' pROXIMO MES NO CALENDARIO
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub NextMonth()
        Me.Month += 1
        If Me.Month > 12 Then
            Me.Month = 1
            Me.Year = Me.Year + 1
        End If
        Me.Day = 1
        Me.dataSelecionada = New Date(Me.Year, Me.Month, Me.Day)
        Me.lblData.Text = MothAndYearSelected(Me.Month, Me.Year)
        CalendarCreate(Me.Day, Me.Month, Me.Year)
        InsertDate(Me.DateV, Me.ColorV)
    End Sub

    ''' <summary>
    ''' MÊS ANTERIOR
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PreviousMonth()
        Me.Month -= 1
        If Me.Month < 1 Then
            Me.Month = 12
            Me.Year = Me.Year - 1
        End If
        Me.Day = 1
        Me.dataSelecionada = New Date(Me.Year, Me.Month, Me.Day)
        Me.lblData.Text = MothAndYearSelected(Me.Month, Me.Year)
        CalendarCreate(Me.Day, Me.Month, Me.Year)
        InsertDate(Me.DateV, Me.ColorV)
    End Sub

    ''' <summary>
    ''' LIMPA DASTAS MARCADAS
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clean()
        Me.xDays = 0
        Me.DateVIndex = 0
        ReDim Me.DateV(0)
        ReDim Me.InfoV(0)
        ReDim Me.ColorV(0)
        CleanColor()
        CalendarCreate(Me.Day, Me.Month, Me.Year)
    End Sub

End Class
