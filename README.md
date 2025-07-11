==============================

📚 Document Library Application

==============================



------------------------------

&nbsp;Instructions: How to Run and Test the Application

------------------------------



&nbsp;BACKEND (ASP.NET Core)

------------------------------

1\. Make sure you have \[.NET 8 SDK or newer](https://dotnet.microsoft.com/) installed.

2\. Open a terminal in the \*\*backend\*\* folder.

3\. Restore dependencies (optional):

&nbsp;  dotnet restore

4\. Run the server:

&nbsp;  dotnet run

5\. Note the server URL in the console (for example: http://localhost:5113).





&nbsp;FRONTEND (React + Vite)

------------------------------

1\. Make sure you have \*\*Node.js 22 or newer\*\* installed.

2\. Open a terminal in the \*\*frontend\*\* folder.

3\. Install dependencies:

&nbsp;  npm install

4\. Start the development server:

&nbsp;  npm run dev

5\. Open your browser at:

&nbsp;  http://localhost:5173





&nbsp;TESTING THE APPLICATION

------------------------------

\- Upload \*\*single\*\* or \*\*multiple\*\* documents.

\- Check that they appear in the \*\*document list\*\*.

\- Click \*\*Download\*\* to test file download.

\- Use \*\*Generate Share Link\*\* and open it in a new tab to verify sharing.





------------------------------

Description of Main Architecture and Design Decisions

------------------------------



FRONTEND

------------------------------

\- \*\*Framework:\*\* React with Vite

\- \*\*Routing:\*\* React Router for page navigation (`/` for upload/list, `/share/:token` for shared access)

\- \*\*Component-based:\*\* 

&nbsp; - DocumentUpload: handles file selection and upload

&nbsp; - DocumentList: fetches and displays all uploaded documents

&nbsp; - ShareLinkAccess: validates and downloads shared links

\- \*\*Styling:\*\* Centralized CSS in App.css for consistent design



\*Design Decision:\* Keep components small and focused for easier maintenance and reuse.





&nbsp;BACKEND

------------------------------

\- \*\*Framework:\*\* ASP.NET Core Minimal APIs

\- \*\*Endpoints:\*\*

&nbsp; - /api/documents/upload – Single file upload

&nbsp; - /api/documents/upload-multiple – Multiple file upload

&nbsp; - /api/documents – List all documents

&nbsp; - /api/documents/{id}/download – Download by ID

&nbsp; - /api/share/{documentId}/create – Create share link

&nbsp; - /api/share/{token}/validate – Validate share link

&nbsp; - /api/share/{token} – Download using share link

&nbsp; - \*\*In-Memory Storage:\*\*

&nbsp; - DocumentStore and ShareLinkStore hold documents and share links temporarily

&nbsp; -  Data is lost on server restart (no database persistence)



\*Design Decision:\* Use in-memory stores for simplicity during development and testing.





&nbsp;INTEGRATION

------------------------------

\- Frontend fetches documents and share link data from the backend using REST APIs.

\- CORS is configured to allow frontend (localhost:5173) to call the backend (localhost:5113).



\*Design Decision:\* Split frontend and backend for clean separation of concerns.





------------------------------

&nbsp;Ideas and Proposals to Improve the Application

------------------------------



&nbsp;FROM A USER PERSPECTIVE

------------------------------

\- Add \*\*authentication\*\* for secure access.

\- Support \*\*user accounts\*\* with personal document libraries.

\- Include \*\*document previews\*\* (e.g., PDF viewer).

\- Allow \*\*folder structures\*\* or \*\*tags\*\* for better organization.

\- Add \*\*drag-and-drop\*\* upload support.

\- Improve \*\*mobile responsiveness\*\* with better layouts.





&nbsp;FROM A TECHNICAL PERSPECTIVE

------------------------------

\- Replace \*\*in-memory\*\* storage with a \*\*real database\*\* (e.g., SQLite, PostgreSQL, MongoDB) for persistence.

\- Add \*\*file storage\*\* on cloud services (AWS S3, Azure Blob, etc.).

\- Implement \*\*JWT-based authentication\*\* for secure APIs.

\- Add \*\*unit tests\*\* and \*\*integration tests\*\* for both frontend and backend.

\- Use \*\*CI/CD pipelines\*\* to deploy automatically.

\- Add \*\*logging\*\* and \*\*error tracking\*\* (e.g., Serilog for backend).

\- Enable \*\*HTTPS\*\* in development and production for security.

\- Optimize \*\*frontend styling\*\* using libraries (Tailwind, Material UI).



