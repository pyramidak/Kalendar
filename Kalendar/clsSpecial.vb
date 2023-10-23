Imports Microsoft.Win32.TaskScheduler

#Region " TaskScheduler "

Class clsTaskScheduler

    Public Name, Description, File, Arguments, Directory As String
    Public Cas As Date

    Sub New()
        Name = "pyramidak " & Application.ExeName
        Description = "Každodenní automatické spouštění pyramidak " & Application.ProductName & " podle nastavení."
        File = If(Application.winStore, Application.ExeName, System.Reflection.Assembly.GetExecutingAssembly().Location)
        Arguments = "-win"
        Cas = Today.AddHours(18).AddMinutes(0)
    End Sub

    Public Sub Create()
        ' Get the service on the local machine
        Try
            Using ts As New TaskService()
                ' Create a new task definition and assign properties
                Dim td As TaskDefinition = ts.NewTask()
                td.RegistrationInfo.Description = Description
                td.Settings.StartWhenAvailable = True

                Dim dt As New DailyTrigger()
                dt.DaysInterval = 1
                dt.StartBoundary = Cas
                'dt.Repetition.Duration = TimeSpan.FromDays(30) 'po dobu
                'dt.Repetition.Interval = TimeSpan.FromMinutes(60) 'každých 
                td.Triggers.Add(dt)
                Dim lt As New LogonTrigger
                lt.Delay = TimeSpan.FromMinutes(3)
                lt.UserId = mySystem.User
                td.Triggers.Add(lt)

                ' Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(New ExecAction(File, Arguments, Directory))

                ' Register the task in the root folder
                ts.RootFolder.RegisterTaskDefinition(Name, td)
            End Using
        Catch ex As Exception
            myRegister.WriteAutoStartMyApp()
        End Try
    End Sub

    Public Sub Delete()
        Try
            Using ts As New TaskService()
                ' Remove the task we just created
                ts.RootFolder.DeleteTask(Name)
            End Using
        Catch ex As Exception
            myRegister.DeleteAutoStartMyApp()
        End Try
    End Sub

    Public Function Exist() As Boolean
        Try
            Using ts As New TaskService()
                'Get an instance of an existing task
                Dim myTask As Task = ts.GetTask(Name)
                If myTask Is Nothing Then Return False

                ' Check to ensure you have a trigger and it is the one want
                If myTask.Definition.Triggers.Count > 0 Then
                    Cas = myTask.Definition.Triggers(0).StartBoundary
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Return myRegister.GetAutoStartMyApp
        End Try
    End Function

End Class

#End Region



