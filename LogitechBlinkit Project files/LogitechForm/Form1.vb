﻿Imports System.Runtime.InteropServices
Imports System.Threading

Public Class Form1

    '-------------------------
    ' Import the functions needed in the project
    ' from the logitech driver dll
    '-------------------------

    <DllImport("LogitechLed.dll")>
    Public Shared Function LogiLedInit() As Boolean
    End Function

    <DllImport("LogitechLed.dll")>
    Public Shared Function LogiLedShutdown() As Boolean
    End Function

    <DllImport("LogitechLed.dll")>
    Public Shared Function LogiLedSetLighting(ByVal red As Integer, ByVal green As Integer, ByVal blue As Integer) As Boolean
    End Function

    ' Load function from the kernel32 library to set the path to the dll used
    ' because the dll from the Logitech driver can be installed in different
    ' locations.
    Public Declare Function SetDllDirectoryA Lib "kernel32" (ByVal lpPathName As String) As Long

    '-------------------------
    ' Define new Thread
    '-------------------------
    Private trd As Thread

    '-------------------------
    ' Function that fires when the form gets loaded
    '-------------------------
    Private Sub Form1_load(sender As Object, e As EventArgs) Handles MyBase.Load


        '-------------------------
        ' After the Label and Picturebox is shown
        ' A new thread is started running
        ' the main program
        ' (fixes a bug where the label and picturebox does not show up)
        '-------------------------
        trd = New Thread(AddressOf Logi)
        trd.Start()
    End Sub

    '-------------------------
    ' Function that fires when the form gets closed
    '-------------------------
    Private Sub Form1_close(sender As Object, e As EventArgs) Handles MyBase.Closed
        '-------------------------
        ' Thread has to be terminated after the form gets closed
        ' else it would run further
        '-------------------------
        trd.Abort()
    End Sub

    Private Sub Logi()

        '-------------------------
        'Define all used Variables
        '-------------------------

        Dim color As String 'color will be split up into their r,g,b components later
        Dim path As String

        Dim r As Integer
        Dim g As Integer
        Dim b As Integer
        Dim delay As Integer
        Dim nrblinks As Integer

        '----------------------------
        ' Read values from txt files
        ' created by the main program
        ' And save them in a variable
        '----------------------------

        color = My.Computer.FileSystem.ReadAllText("config\logitechcolor.txt")
        delay = My.Computer.FileSystem.ReadAllText("config\logitechdelay.txt")
        nrblinks = My.Computer.FileSystem.ReadAllText("config\logitechnrblinks.txt")
        path = My.Computer.FileSystem.ReadAllText("config\logitechpath.txt")

        '-------------------------
        ' Convert the string "color" into three Integers 
        ' which are three characters each
        ' e. g. :
        ' string color = "123 213 100"
        ' int r = 123; int g = 213; int b = 100 
        '-------------------------
        ' If the value is smaller then 100 it is saved inside the
        ' txt file with "0"s In front:
        ' 5 -> 005
        '-------------------------
        r = CInt(color.Chars(0) + color.Chars(1) + color.Chars(2))
        g = CInt(color.Chars(4) + color.Chars(5) + color.Chars(6))
        b = CInt(color.Chars(8) + color.Chars(9) + color.Chars(10))

        '-------------------------
        ' Because the LogitechLedSetLightning function needs values as parameters 
        ' between 0 to 100 as a symbol for % brightness
        ' the rgb values are converted to a fitting value
        '-------------------------

        r = (r / 255) * 100
        g = (g / 255) * 100
        b = (b / 255) * 100


        '-------------------------
        ' Use the function from the kernel32 library
        ' It adds the path to the dll to a list. So when Windows tries to find the dll
        ' it first look at the given path
        '-------------------------
        SetDllDirectoryA(path)

        '-------------------------
        ' Function call from the Logitech Dll which initialise the Keyboard. 
        ' So we have access to it and no other program have access
        '-------------------------
        LogiLedInit()

        '-------------------------
        'Because the LogiLedInit function runs in another thread the program have to wait a little bit for it to finish
        '-------------------------
        Threading.Thread.Sleep(200)

        '-------------------------
        ' Repeat 'nrblinks' times:
        ' - Set the color of the keyboard to the r,g,b values
        ' - Wait 'delay' ms
        ' - Set the color of the keyboard to 0 -> off
        ' - Wait 'delay' ms
        '-------------------------

        For i As Integer = 1 To nrblinks
            LogiLedSetLighting(r, g, b)
            Threading.Thread.Sleep(delay)
            LogiLedSetLighting(0, 0, 0)
            Threading.Thread.Sleep(delay)
        Next

        '-------------------------
        ' Disconnect the connection to the Keyboard
        ' Quit Application
        '-------------------------

        LogiLedShutdown()
        Application.Exit()
    End Sub
End Class