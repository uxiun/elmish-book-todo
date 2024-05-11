# Caution
## `pnpm run dev` might not work
In my case, fable compile triggered by every change after `pnpm run dev` (alias for `fable watch --run vite`) never complete for some reason.
Therefore I run both `pnpm run start` (alias for `vite`), `pnpm run watch`(alias for `fable watch`) on different terminal...

## `pnpm run fix`
`pnpm run build` generates `dist/`. Despite setting `"homepage": "."` in `package.json`, `dist/index.html`'s `<script src=>` path still starts with `/`.
`pnpm run fix` (alias for `sed` command) removes that `/` to avoid `NotFound` error, make the builds correctly works.