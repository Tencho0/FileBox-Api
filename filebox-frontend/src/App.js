import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { FileList } from './components/FileList';
import { FileUpload } from './components/FileUpload';

function App() {
  const [files, setFiles] = useState([]);

  const fetchFiles = async () => {
    try {
      const response = await axios.get('https://localhost:7087/api/files');
      setFiles(response.data);
    } catch (err) {
      console.error('Failed to fetch files', err);
    }
  };

  useEffect(() => {
    fetchFiles();
  }, []);

  return (
    <div className="App">
      <h1>FileBox</h1>
      <FileUpload fetchFiles={fetchFiles} />
      <FileList files={files} fetchFiles={fetchFiles} />
    </div>
  );
}

export default App;
