# Caution
## `pnpm run dev` might not work
In my case, fable compile by `pnpm run dev` never complete for some reason.
Therefore I run both `pnpm run start`, `pnpm run watch` on different terminal...

## `pnpm run fix`
`pnpm run build` generates `dist/`. Despite setting `"homepage": "."` in `package.json`, `dist/index.html`'s `<script src=>` path still starts with `/`.
`pnpm run fix` removes that `/` to avoid `NotFound` error, make the builds correctly works.