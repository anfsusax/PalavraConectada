-- Consultas SQL para verificar versículos da versão ACF (Almeida Corrigida Fiel)

-- 1. Contar total de versículos da versão ACF
SELECT COUNT(*) as TotalVersiculosACF
FROM Verses
WHERE Version = 'acf';

-- 2. Ver alguns versículos da versão ACF (primeiros 10)
SELECT 
    BookName,
    Chapter,
    Number,
    Text,
    Version
FROM Verses
WHERE Version = 'acf'
ORDER BY BookName, Chapter, Number
LIMIT 10;

-- 3. Contar versículos por versão (todas as versões)
SELECT 
    Version,
    COUNT(*) as TotalVersiculos
FROM Verses
GROUP BY Version
ORDER BY TotalVersiculos DESC;

-- 4. Verificar se há versículos ACF de um livro específico (ex: Gênesis)
SELECT 
    BookName,
    Chapter,
    COUNT(*) as TotalVersiculos
FROM Verses
WHERE Version = 'acf' AND BookName = 'Gênesis'
GROUP BY BookName, Chapter
ORDER BY Chapter
LIMIT 5;

-- 5. Buscar versículos ACF que contenham uma palavra específica
SELECT 
    BookName,
    Chapter,
    Number,
    Text
FROM Verses
WHERE Version = 'acf' 
  AND Text LIKE '%amor%'
LIMIT 10;

