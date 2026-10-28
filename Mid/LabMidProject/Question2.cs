// using System;

// class Question2
// {
//     static void Main()
//     {
//         int[,] marks = new int[3, 4];
//         int[] total = new int[3];
//         double[] average = new double[3];

//         // Input marks of 3 students in 4 subjects
//         for (int i = 0; i < 3; i++)
//         {
//             Console.WriteLine($"\nEnter marks for Student {i + 1}:");

//             for (int j = 0; j < 4; j++)
//             {
//                 Console.Write($"Subject {j + 1}: ");
//                 marks[i, j] = Convert.ToInt32(Console.ReadLine());
//             }
//         }

//         // Calculate total and average of each student
//         for (int i = 0; i < 3; i++)
//         {
//             for (int j = 0; j < 4; j++)
//             {
//                 total[i] += marks[i, j];
//             }

//             average[i] = total[i] / 4.0;
//         }

//         // Display student results in table format
//         Console.WriteLine("\nSTUDENT MARKS REPORT");
//         Console.WriteLine(
//             "Student\tSub1\tSub2\tSub3\tSub4\tTotal\tAverage");

//         for (int i = 0; i < 3; i++)
//         {
//             Console.Write($"Student {i + 1}\t");

//             for (int j = 0; j < 4; j++)
//             {
//                 Console.Write(marks[i, j] + "\t");
//             }

//             Console.WriteLine($"{total[i]}\t{average[i]:F2}");
//         }

//         // Find highest marks in each subject
//         Console.WriteLine("\nHIGHEST MARKS IN EACH SUBJECT");

//         for (int j = 0; j < 4; j++)
//         {
//             int highest = marks[0, j];

//             for (int i = 1; i < 3; i++)
//             {
//                 if (marks[i, j] > highest)
//                 {
//                     highest = marks[i, j];
//                 }
//             }

//             Console.WriteLine($"Subject {j + 1}: {highest}");
//         }
//     }
// }