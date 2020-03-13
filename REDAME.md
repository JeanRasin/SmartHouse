
��������� ����� � �������.
docker build -f DockerfileTests -t tests-smart-house . 

��������� ��� ����� � �������
docker run -it --rm --name tests-smart-house tests-smart-house dotnet vstest TestRepository/RepositoryTest.dll TestBusiness/BusinessTest.dll TestApi/ApiTest.dll TestApiIntegration/ApiIntegrationTest.dll TestServices/ServicesTest.dll


��������� ����������������� ����������
docker-compose -f docker-compose.yml up -d

��������� ����������������� ���������� � �����
docker-compose -f docker-compose.yml -f docker-compose.test.yml up -d

��������� ������ �����
docker-compose -f docker-compose.test.yml up -d