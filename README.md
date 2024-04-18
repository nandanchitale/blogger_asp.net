
# Blogger Website using Asp.net Core

This project is a blog platform developed using ASP.NET Core MVC. It provides users with the ability to create, read, update, and delete blog posts. The platform includes features such as user authentication, post commenting, and pagination.

Key Features:

User Authentication: Users can sign up and log in to create and manage their blog posts. Authentication is handled securely using ASP.NET Core Identity.

Blog Post Management: Authenticated users can create, edit, and delete their blog posts. Each post includes a title, content, author information, and timestamp.

Commenting System: Users can comment on blog posts after logging in. Comments are displayed below each blog post and include the commenter's name, timestamp, and comment text.

Pagination: The blog posts are paginated to improve usability and navigation. Users can easily browse through multiple pages of posts.

Error Handling: The platform includes robust error handling to ensure smooth user experience. Errors are logged for debugging purposes, and appropriate error messages are displayed to users when necessary.

Technologies Used:

ASP.NET Core MVC,
Entity Framework Core,
Razor Pages,
ASP.NET Core Identity,
HTML/CSS/JavaScript,
Bootstrap,
Postgresql

Getting Started:

Clone the repository to your local machine.
Install the necessary dependencies using NuGet Package Manager.
Set up the database using Entity Framework Core migrations.
Configure the application settings, such as database connection strings.
Run the application locally using Visual Studio or the .NET CLI.
Access the application through the specified URL and start exploring the features.
Contributing:

Contributions are welcome! If you have any suggestions, bug fixes, or new features to add, please open an issue or submit a pull request. Let's make this project better together.

License:

This project is licensed under the MIT License. Feel free to use and modify it for your own purposes.

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

## Deployment

Assumptions : 
 - Dotnet 8 installed on the system.
 - Docker Installed (for postgresql database). If you have postgresql database locally installed, then modify the appsettings.json file in the project directory as required.

To deploy this project run

1> Clone Github Repository
```bash 
git clone https://github.com/nandanchitale/blogger_asp.net blogger_asp_net && cd blogger_asp_net
```

2> Start Postgresql database container (skip if you have database installed locally)
```bash
docker compose up --build -d
```

3> Start the application
```bash
dotnet run --project .\Blogger\Blogger.csproj
```

## Tech Stack

**Client:** Razor Pages, Bootstraps 5

**Server:** Asp.Net core 8 MVC, Postgresql

