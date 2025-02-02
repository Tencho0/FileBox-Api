import React from 'react';
import axios from 'axios';
import { API_BASE_URL } from '../config';

export const FileList = ({ files, fetchFiles }) => {
    const handleDelete = async (id) => {
        try {
            await axios.delete(`${API_BASE_URL}/files/${id}`);
            fetchFiles();
        } catch (err) {
            console.error('Failed to delete file', err);
        }
    };

    return (
        <ul>
            {files.map((file) => (
                <li key={file.id}>
                    {file.name}.{file.extension} - {new Date(file.uploadedAt).toLocaleString()}
                    <button onClick={() => handleDelete(file.id)}>Delete</button>
                </li>
            ))}
        </ul>
    );
};