import React, { useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import DocumentUpload from './components/DocumentUpload';
import DocumentList from './components/DocumentList';
import ShareLinkAccess from './pages/ShareLinkAccess';
import './App.css';
function App() {
  const [refreshList, setRefreshList] = useState(0);

  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/"
          element={
            <div >
              <h1>📚 Document Library Frontend</h1>
              <DocumentUpload onUploadComplete={() => setRefreshList(prev => prev + 1)} />
              <DocumentList refreshKey={refreshList} 
              onRequestRefresh={() => setRefreshList(prev => prev + 1)}
              />
            </div>
          }
        />
        <Route path="/share/:token" element={<ShareLinkAccess />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
