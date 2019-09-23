Imports System.Windows.Forms
Imports System.Drawing

''' <summary>
''' CALENDÁRIO CUSTOMIZADO EM DATAGRIDVIEW
''' AUTHOR: WALFRIDO OLIVEIRA DA SILVA
''' DATA: 22/01/115
''' </summary>
''' <remarks></remarks>


Public Class GridCalendar

    Private Month As Integer = Today.Month
    Private Year As Integer = Today.Year
    Private Day As Integer = Today.Day
    Private xDays As Integer = 0
    Private blnColor As Boolean = False
    Private blnNextMonth As Boolean = False
    Private DateV() As Date = {}
    Private DateVIndex As Integer = 0
    Private strInfos() As String = {}
    Private grid As DataGridView

    'VARIÁVEIS DE COLORES DO CALENDÁRIO
    Private ColorOfMonth As System.Drawing.Color = Color.Black
    Private ColorOfNextMonth As System.Drawing.Color = Color.Gray
    Private ColorOfPrevious As System.Drawing.Color = Color.Gray
    Private ColorOfWeekend As System.Drawing.Color = Color.LightGray
    Private COlorOfDetachDate As System.Drawing.Color = Color.Red

    ''' <summary>
    ''' ADIIOCIONA DATA AO VETOR DE DATAS
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddDay(ByVal data As Date) As Boolean
        DateV(DateVIndex) = data
        DateVIndex += 1
        ReDim Preserve DateV(DateVIndex)
    End Function

    Public Sub New(ByRef grid As DataGridView)
        Me.grid = grid
        CriaCalendario(Me.grid)
    End Sub

    Public Property GetOrSetMonth() As Integer
        Get
            Return Me.Month
        End Get
        Set(ByVal value As Integer)
            Me.Month = value
        End Set
    End Property

    Public Property GetOrSetYear() As Integer
        Get
            Return Me.Year
        End Get
        Set(ByVal value As Integer)
            Me.Year = value
        End Set
    End Property

    Public Property GetOrSetDay() As Integer
        Get
            Return Me.Day
        End Get
        Set(ByVal value As Integer)
            Me.Day = value
        End Set
    End Property


    '''<summary>
    ''' ADICIONA SE NECESSÁRIO LINHAS QUE IRÃO COMPOR O CALENDÁRIO
    ''' 6X7
    ''' </summary>
    ''' <param name="DataGridView">DATAGRIDVIEW QUE SERÁ ADICIONADO AS LINHAS</param>
    ''' <returns>TRUE OK FALSE ERRO</returns>
    ''' <remarks></remarks>
    Public Function CriaCalendario(ByRef DataGridView As DataGridView) As Boolean
        Dim intRows As Integer = 0
        Try
            If DataGridView.RowCount = 0 Then
                While intRows < 6
                    DataGridView.Rows.Add(String.Empty)
                    intRows += 1
                End While
            End If
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' RETORNA A DATA ATUAL POR EXTENSO
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToday() As String
        Return WeekdayName(Today.DayOfWeek + 1).ToUpper & ", " _
                         & Today.Day.ToString & " DE " _
                         & MonthName(Today.Month).ToUpper _
                         & " DE " & Today.Year.ToString
    End Function

    ''' <summary>
    ''' LIMPA AS CORES MODIFICADOS DO DATAGRIDVIEW
    ''' </summary>
    ''' <param name="DataGridView">DATAGRIDVIEW QUE SERÁ LIMPO</param>
    ''' <remarks></remarks>
    Private Sub CleanColor(ByRef DataGridView As DataGridView)
        For Each rows As DataGridViewRow In DataGridView.Rows
            For Each cells As DataGridViewCell In rows.Cells
                cells.Style.ForeColor = ColorOfMonth
                cells.Style.BackColor = Color.White
            Next
        Next
    End Sub

    ''' <summary>
    ''' LIMPA CELULAS SELECIONADAS E SELECIONA O DIA ATUAL
    ''' </summary>
    ''' <param name="DataGridView"></param>
    ''' <remarks></remarks>
    Public Sub SelectedToday(ByRef DataGridView As DataGridView)
        DataGridView.ClearSelection()

        For i As Integer = 0 To DataGridView.RowCount - 1
            For j As Integer = 0 To DataGridView.ColumnCount - 1
                If j > 0 Then
                    If CInt(DataGridView.Rows(i).Cells(j).Value) < CInt(DataGridView.Rows(i).Cells(j - 1).Value) Then
                        Continue For
                    Else
                        If CInt(DataGridView.Rows(i).Cells(j).Value) = Today.Day Then
                            DataGridView.Rows(i).Cells(j).Selected = True
                        End If
                    End If
                Else
                    If CInt(DataGridView.Rows(i).Cells(j).Value) = Today.Day Then
                        DataGridView.Rows(i).Cells(j).Selected = True
                    End If
                End If
            Next
        Next

    End Sub

    ''' <summary>
    ''' RETORNA O MÊS E O ANO DA DATA SELECIONADA
    ''' </summary>
    ''' <param name="Month"></param>
    ''' <param name="Year"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function MothAndYearSelected(ByVal Month, ByVal Year) As String
        Dim strMonthYear As String = String.Empty

        'PREENCHE LABEL COM O MES E ANO ATUAL
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
        strMonthYear += " " & Year

        Return strMonthYear
    End Function

    ''' <summary>
    '''  FUNÇÃO PRINCIPAL ONDE É FORMATADO O CALENDÁRIO
    ''' </summary>
    ''' <param name="Day">DIA CORRENTE</param>
    ''' <param name="Month">MES CORRENTE</param>
    ''' <param name="Year">ANO CORRENTE</param>
    ''' <param name="DataGridView">DATAGRIDVIEW QUE SERÃO ADICIONADO AS INFORMAÇÕES</param>
    ''' <remarks></remarks>
    Public Sub CalendarCreate(ByVal Day As Integer, _
                              ByVal Month As Integer, _
                              ByVal Year As Integer, _
                              ByRef DataGridView As DataGridView)
        CleanColor(DataGridView)

        blnColor = False

        'OBTÉM O DIA DA SEMANA DO PRIMEIRO DO DIA DO MÊS CORRENTE
        Dim DayOfWeek As Integer = CDate("01/" & CStr(Month) & "/" & CStr(Year)).DayOfWeek

        ' OBTÉM A QUANTIDADE DIAS DO MÊS CORRENTE
        Dim DaysOfMonth As Integer = Date.DaysInMonth(Year, Month)

        'OBTÉM A QUANTIDADE DIAS DO MÊS ANTERIOR AO CORRENTE
        Dim PreviousMonth As Integer = Month - 1
        Dim PreviousYear As Integer = Year
        If PreviousMonth < 1 Then
            PreviousMonth = 12
            PreviousYear = Year - 1
        End If
        Dim PreviousDaysMonth As Integer = Date.DaysInMonth(PreviousYear, PreviousMonth)

        'OBTÉM A QUANTIDADE DE DIAS DO MÊS SEGUINTE AO O CORRENTE
        Dim NextMonth As Integer = Month + 1
        Dim NextYear As Integer = Year
        If NextMonth > 12 Then
            NextMonth = 1
            NextYear = Year + 1
        End If
        Dim DiasMesProx As Integer = Date.DaysInMonth(NextYear, NextMonth)

        'PREENCHE A CELULA RESPECTIVA COM O PRIMEIRO DIA DO MÊS CORRENTE
        Select Case DayOfWeek
            Case 0
                DataGridView.Rows(0).Cells(0).Value = 1
            Case 1
                DataGridView.Rows(0).Cells(1).Value = 1
            Case 2
                DataGridView.Rows(0).Cells(2).Value = 1
            Case 3
                DataGridView.Rows(0).Cells(3).Value = 1
            Case 4
                DataGridView.Rows(0).Cells(4).Value = 1
            Case 5
                DataGridView.Rows(0).Cells(5).Value = 1
            Case 6
                DataGridView.Rows(0).Cells(6).Value = 1
        End Select

        'PREENCHE O MES ANTERIOR SE NECESSITAR
        xDays = DayOfWeek - 1
        While xDays >= 0
            DataGridView.Rows(0).Cells(xDays).Value = PreviousDaysMonth
            DataGridView.Rows(0).Cells(xDays).Style.ForeColor = ColorOfNextMonth

            PreviousDaysMonth -= 1
            xDays -= 1
        End While

        'PREENCHE OS DIAS DO MÊS CORRENTE
        xDays = 2
        For Each rows As DataGridViewRow In DataGridView.Rows
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
                    cells.Value = xDays
                    xDays += 1
                End If
            Next
        Next

        'DISABILITA O REDIMENSIONAMENTO DO DATAGRIDVIEW
        For x As Int32 = 0 To DataGridView.ColumnCount - 1
            DataGridView.Columns(x).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    ''' <summary>
    ''' MARCA DATAS SELECIONADAS NO DATAGRID COM FUNDO DE COR DIFERENCIADO
    ''' </summary>
    ''' <param name="DataGridView">DATAGRID A SER MANIPULADO</param>
    ''' <param name="DateOfDay">DATA A SER MARCADA</param>
    ''' <remarks></remarks>
    Private Sub DetachDate(ByRef DataGridView As DataGridView, ByVal DateOfDay As Date, _
                                     ByVal color As System.Drawing.Color)

        blnNextMonth = False

        'OBTÉM O DIA DA SEMANA DO PRIMEIRO DO DIA DO MÊS CORRENTE
        Dim DayOfWeek As Integer = CDate(CStr("01/" & DateOfDay.Month & "/" & CStr(DateOfDay.Year))).DayOfWeek

        For Each rows As DataGridViewRow In DataGridView.Rows
            For Each cells As DataGridViewCell In rows.Cells
                If cells.RowIndex > 0 And cells.Value = 1 Then
                    blnNextMonth = True
                End If

                If DateOfDay.Month = Month And DateOfDay.Year = Year Then

                    If cells.RowIndex = 0 And cells.ColumnIndex >= DayOfWeek Then
                        If DateOfDay.Day = cells.Value Then
                            cells.Style.BackColor = color
                            Exit Sub
                        End If

                    ElseIf cells.RowIndex > 0 Then
                        If DateOfDay.Day = cells.Value Then
                            cells.Style.BackColor = color
                            Exit Sub
                        End If
                    End If

                ElseIf DateOfDay.Month = Month - 1 And DateOfDay.Year = Year Then

                    If cells.RowIndex = 0 And (cells.ColumnIndex < DayOfWeek And Not rows.Cells(0).Value = 1) Then

                        If DateOfDay.Day = cells.Value Then
                            cells.Style.BackColor = color
                            Exit Sub
                        End If
                    End If

                ElseIf DateOfDay.Month = Month + 1 And DateOfDay.Year = Year Then
                    If cells.RowIndex > 0 And blnNextMonth Then
                        If DateOfDay.Day = cells.Value Then
                            cells.Style.BackColor = color
                            Exit Sub
                        End If
                    End If

                ElseIf DateOfDay.Month = 12 And DateOfDay.Year = Year - 1 Then
                    If cells.RowIndex = 0 And cells.ColumnIndex < DayOfWeek Then
                        If DateOfDay.Day = cells.Value Then
                            cells.Style.BackColor = color
                            Exit Sub
                        End If
                    End If

                ElseIf DateOfDay.Month - Month = -11 And DateOfDay.Year = Year + 1 Then
                    If cells.RowIndex > 0 And blnNextMonth Then
                        If DateOfDay.Day = cells.Value Then
                            cells.Style.BackColor = color
                            Exit Sub
                        End If
                    End If
                End If

            Next
        Next
    End Sub

    ''' <summary>
    ''' CAPTURA O VETOR DE DATAS E INSERE NO DATAGRIDVIEW O DESTAQUE
    ''' ATRAVES DA FUNÇÃO spiDetachDate
    ''' </summary>
    ''' <param name="Dates">VETOR DE DATAS</param>
    ''' <remarks></remarks>
    Public Sub InsertDate(ByVal Dates As Date(), _
                          ByVal color() As System.Drawing.Color)

        Dim x As Integer = 0
        While x < color.Length
            DetachDate(Dates(x), color(x))
            x += 1
        End While

    End Sub
End Class
