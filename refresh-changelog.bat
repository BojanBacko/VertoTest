cd ./
echo "Verto CMS: Changelog" > changelog.csv
echo "" >> changelog.csv
echo "Commit Hash","Author","Email","Date","Subject" >> changelog.csv
git.exe log --pretty=format:"%%H","%%an","%%ae","%%ad","%%s%%x0D" >> changelog.csv
start changelog.csv