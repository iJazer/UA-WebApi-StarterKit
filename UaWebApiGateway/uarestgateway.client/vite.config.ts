import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';

const baseFolder =
   process.env.APPDATA !== undefined && process.env.APPDATA !== ''
      ? `${process.env.APPDATA}/ASP.NET/https`
      : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : "uarestgateway.client";

if (!certificateName) {
   console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
   process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
   if (0 !== child_process.spawnSync('dotnet', [
      'dev-certs',
      'https',
      '--export-path',
      certFilePath,
      '--format',
      'Pem',
      '--no-password',
   ], { stdio: 'inherit', }).status) {
      throw new Error("Could not create certificate.");
   }
}

// https://vitejs.dev/config/
export default defineConfig({
   plugins: [plugin()],
   resolve: {
      alias: {
         '@': fileURLToPath(new URL('./src', import.meta.url))
      }
    },
    build: {
        sourcemap: true
    },
   server: {
      proxy: {
         '^/api': {
            target: 'https://localhost:7010/',
            secure: false
         },
         '^/opcua': {
            target: 'https://localhost:7010/',
            secure: false
         },
         '^/data': {
            target: 'https://localhost:7010/',
            secure: false
         },
         '^/stream': {
            target: 'wss://localhost:7010/',
            ws: true,
            secure: false
           },
        '^/api/aas': {
            target: 'https://localhost:7010/',
            secure: false
           },
         '^/api/v3.0': {
            target: 'https://localhost:7010/',
            secure: false
         }
      },
      port: 44430,
      https: {
         key: fs.readFileSync(keyFilePath),
         cert: fs.readFileSync(certFilePath),
      }
   }
})
