const BASE_URL = 'http://localhost:5113/api';

// Get all documents

export async function getDocuments() {
  const res = await fetch(`${BASE_URL}/documents`);
  if (!res.ok) throw new Error('Failed to fetch documents');
  return res.json();
}

// Upload single document

export async function uploadDocument(file) {
  const formData = new FormData();
  formData.append('file', file);

  const res = await fetch(`${BASE_URL}/documents/upload`, {
    method: 'POST',
    body: formData,
  });

  if (!res.ok) throw new Error('Failed to upload document');
  return res.json();
}


// Upload multiple documents

export async function uploadMultipleDocuments(files) {
  console.log(files);
  const formData = new FormData();

  if(files.length > 0) {
        for(var x = 0; x < files.length; x++) {
            formData.append('files', files[x]);    
        }
    } 

  const res = await fetch(`${BASE_URL}/documents/upload-multiple`, {
    method: 'POST',
    body: formData,
  });

  if (!res.ok) throw new Error('Failed to upload documents');
  return res.json();
}


// Download document (return URL for direct download)

export function getDownloadUrl(id) {
  return `${BASE_URL}/documents/${id}/download`;
}


// Create a share link

export async function createShareLink(documentId, expiresInHours) {
  const res = await fetch(`${BASE_URL}/share/${documentId}/create?expiresInHours=${expiresInHours}`, {
    method: 'POST'
  });
  if (!res.ok) throw new Error('Failed to create share link');
  return res.json();
}


// Access shared document via token

export async function accessSharedLink(token) {
  const res = await fetch(`${BASE_URL}/share/${token}`);
  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.title || data.message || 'Server error');
  }

  // Even with res.ok, check if it contains error fields
  if (data.status >= 400 || data.title || data.message) {
    throw new Error(data.title || data.message || 'Invalid or expired link');
  }

  return data;
}