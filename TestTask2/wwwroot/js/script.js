const apiEmployee = 'api/Employee';
const apiCompany = 'api/Company';
const apiProject = 'api/Project';
const apiProjectEmployee = 'api/ProjectEmployee';

document.addEventListener("DOMContentLoaded", async () => {
    await loadEmployees();
    await loadCompanies();
    await loadProjects();
});

//CRUD скирпты для сотрудника
async function loadEmployees() {
    try {
        const response = await fetch(apiEmployee);
        const data = await response.json();
        console.log(data);
        
        const employeeBlock = document.querySelector(".employeeBlock");
        employeeBlock.innerHTML = `
            <button class="btn btn-primary mb-3" onclick="showCreateModal()">Создать сотрудника</button>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Имя</th>
                        <th>Фамилия</th>
                        <th>Отчество</th>
                        <th>Email</th>
                        <th>Действия</th>
                    </tr>
                </thead>
                <tbody>
                    ${data.map(emp => `
                        <tr>
                            <td>${emp.id}</td>
                            <td>${emp.firstName}</td>
                            <td>${emp.lastName}</td>
                            <td>${emp.middleName}</td>
                            <td>${emp.email}</td>
                            <td>
                                <button class="btn btn-warning btn-sm" onclick="editEmployee(${emp.id})">Редактировать</button>
                                <button class="btn btn-danger btn-sm" onclick="deleteEmployee(${emp.id})">Удалить</button>
                            </td>
                        </tr>
                    `).join('')}
                </tbody>
            </table>
        `;
    }catch (error) {
        console.error('Ошибка при загрузке сотрудников:', error);
    }
}

async function showCreateModal() {
    const modalHtml = `
        <div class="modal fade" id="employeeModal" tabindex="-1" aria-labelledby="employeeModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="employeeModalLabel">Создать сотрудника</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                    </div>
                    <div class="modal-body">
                        <form id="employeeForm">
                            <div class="mb-3">
                                <label for="FirstName" class="form-label">Имя</label>
                                <input type="text" class="form-control" id="FirstName">
                                <div class="validation-error text-danger"></div>
                            </div>
                            <div class="mb-3">
                                <label for="LastName" class="form-label">Фамилия</label>
                                <input type="text" class="form-control" id="LastName">
                                <div class="validation-error text-danger"></div>
                            </div>
                            <div class="mb-3">
                                <label for="MiddleName" class="form-label">Отчество</label>
                                <input type="text" class="form-control" id="MiddleName">
                                <div class="validation-error text-danger"></div>
                            </div>
                            <div class="mb-3">
                                <label for="Email" class="form-label">Email</label>
                                <input type="email" class="form-control" id="Email">
                                <div class="validation-error text-danger"></div>
                            </div>
                            <button type="submit" class="btn btn-primary">Создать</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', modalHtml);
    const modal = new bootstrap.Modal(document.getElementById('employeeModal'));
    modal.show();

    document.getElementById('employeeForm').onsubmit = async (e) => {
        e.preventDefault();
       clearValidationErrors();
       await createEmployee(modal);
    };
}

async function createEmployee(modal) {
    clearValidationErrors();

    const employee = {
        firstName: document.getElementById('FirstName').value,
        lastName: document.getElementById('LastName').value,
        middleName: document.getElementById('MiddleName').value,
        email: document.getElementById('Email').value
    };

    try {
        const response = await fetch(apiEmployee, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(employee)
        });

        if (!response.ok) {
            if (response.status === 400) {
                const errors = await response.json();
                console.log(errors);
                displayValidationErrors(errors);
            } else {
                throw new Error('Ошибка при создании сотрудника');
            }
        }else{
            modal.hide();
            await loadEmployees();
            document.getElementById('employeeModal').remove();
        }
    } catch (error) {
        console.error('Ошибка при создании сотрудника:', error);
    }
}

async function deleteEmployee(id) {
    if (confirm('Вы уверены, что хотите удалить этого сотрудника?')) {
        try {
            const response = await fetch(`${apiEmployee}/${id}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                console.error('Ошибка при удалении сотрудника');
            } else {
                await loadEmployees();
            }
        } catch (error) {
            console.error('Ошибка при удалении сотрудника:', error);
        }
    }
}

async function editEmployee(id) {
    try {
        const response = await fetch(`${apiEmployee}/${id}`);
        const data = await response.json();

        removeExistingModal('employeeModal');

        const modalHtml = `
            <div class="modal fade" id="employeeModal" tabindex="-1" aria-labelledby="employeeModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="employeeModalLabel">Редактировать сотрудника</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                        </div>
                        <div class="modal-body">
                            <form id="employeeForm">
                                <div class="mb-3">
                                    <label for="FirstName" class="form-label">Имя</label>
                                    <input type="text" class="form-control" id="FirstName" value="${data.firstName}">
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label for="LastName" class="form-label">Фамилия</label>
                                    <input type="text" class="form-control" id="LastName" value="${data.lastName}">
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label for="MiddleName" class="form-label">Отчество</label>
                                    <input type="text" class="form-control" id="MiddleName" value="${data.middleName}">
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label for="Email" class="form-label">Email</label>
                                    <input type="email" class="form-control" id="Email" value="${data.email}">
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <button type="submit" class="btn btn-primary">Обновить</button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        `;

        document.body.insertAdjacentHTML('beforeend', modalHtml);
        const modal = new bootstrap.Modal(document.getElementById('employeeModal'));
        modal.show();
        document.getElementById('employeeForm').onsubmit = async (e) => {
            e.preventDefault();
            clearValidationErrors();
            await updateEmployee(id, modal);
        };

    } catch (error) {
        console.error('Ошибка при редактировании сотрудника:', error);
    }
}
async function updateEmployee(id, modal) {
    const updatedEmployee = {
        firstName: document.getElementById('FirstName').value,
        lastName: document.getElementById('LastName').value,
        middleName: document.getElementById('MiddleName').value,
        email: document.getElementById('Email').value
    };

    try {
        const response = await fetch(`${apiEmployee}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedEmployee)
        });

        if (!response.ok) {
            if (response.status === 400) {
                const errors = await response.json();
                console.log(errors);
                displayValidationErrors(errors);
            } else {
                console.error('Ошибка при обновлении сотрудника');
            }
        } else {
            modal.hide();
            await loadEmployees();
            document.getElementById('employeeModal').remove();
        }
    } catch (error) {
        console.error('Ошибка при обновлении сотрудника:', error);
    }
}

//CRUD скирпты для Компании
async function loadCompanies(){
    try {
        const response = await fetch(apiCompany);
        const data = await response.json();
        const companyBlock = document.querySelector(".companyBlock");
        companyBlock.innerHTML = `
            <button class="btn btn-primary mb-3" onclick="showCreateCompanyModal()">Создать компанию</button>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Название</th>
                        <th>Действия</th>
                    </tr>
                </thead>
                <tbody>
                    ${data.map(comp => `
                        <tr>
                            <td>${comp.id}</td>
                            <td>${comp.name}</td>
                            <td>
                                <button class="btn btn-warning btn-sm" onclick="editCompany(${comp.id})">Редактировать</button>
                                <button class="btn btn-danger btn-sm" onclick="deleteDeleteCompany(${comp.id})">Удалить</button>
                            </td>
                        </tr>
                    `).join('')}
                </tbody>
            </table>
        `;
    }catch (error){
        console.error('Ошибка при загрузке компаний:', error);
    }
}

async function showCreateCompanyModal() {
    const modalHtml = `
        <div class="modal fade" id="companyModal" tabindex="-1" aria-labelledby="companyModalModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="companyModalLabel">Создать сотрудника</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                    </div>
                    <div class="modal-body">
                        <form id="companyForm">
                            <div class="mb-3">
                                <label for="Name" class="form-label">Имя</label>
                                <input type="text" class="form-control" id="Name">
                                <div class="validation-error text-danger"></div>
                            </div>
                            <button type="submit" class="btn btn-primary">Создать</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', modalHtml);
    const modal = new bootstrap.Modal(document.getElementById('companyModal'));
    modal.show();

    document.getElementById('companyForm').onsubmit = async (e) => {
        e.preventDefault();
        clearValidationErrors();
        await createCompany(modal);
    };
}

async function createCompany(modal) {
    clearValidationErrors();
    
    const company = {
        Name: document.getElementById('Name').value,
    };

    try {
        const response = await fetch(apiCompany, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(company)
        });

        if (!response.ok) {
            if (response.status === 400) {
                const errors = await response.json();
                console.log(errors);
                displayValidationErrors(errors);
            } else {
                throw new Error('Ошибка при создании компании');
            }
        }else{
            modal.hide();
            await loadCompanies();
            document.getElementById('companyModal').remove();
        }
    } catch (error) {
        console.error('Ошибка при создании сотрудника:', error);
    }
}

async function deleteDeleteCompany(id) {
    if(confirm("Вы уверены что хотите удалить эту компанию?")){
        try {
            const response = await fetch(`${apiCompany}/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) {
                console.error('Ошибка при удалении сотрудника');
            } else {
                await loadCompanies();
            }
        }catch (error) {
            console.error('Ошибка при удалении компании:', error);
        }
    }
}

async function editCompany(id) {
    try {
        const response = await fetch(`${apiCompany}/${id}`);
        const data = await response.json();
        removeExistingModal('companyModal');

        const modalHtml = `
        <div class="modal fade" id="companyModal" tabindex="-1" aria-labelledby="companyModalModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="companyModalLabel">Создать сотрудника</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                    </div>
                    <div class="modal-body">
                        <form id="companyForm">
                            <div class="mb-3">
                                <label for="Name" class="form-label">Имя</label>
                                <input type="text" class="form-control" id="Name" value="${data.name}">
                                <div class="validation-error text-danger"></div>
                            </div>
                            <button type="submit" class="btn btn-primary">Создать</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    `;

        document.body.insertAdjacentHTML('beforeend', modalHtml);
        const modal = new bootstrap.Modal(document.getElementById('companyModal'));
        modal.show();
        document.getElementById('companyForm').onsubmit = async (e) => {
            e.preventDefault();
            clearValidationErrors();
            await updateCompany(id, modal);
        };
    }catch (error) {
        console.error('Ошибка при редактировании компании:', error);
    }
}

async function updateCompany(id, modal) {
    const updatedEmployee = {
        Name: document.getElementById('Name').value,
    };

    try {
        const response = await fetch(`${apiEmployee}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedEmployee)
        });

        if (!response.ok) {
            if (response.status === 400) {
                const errors = await response.json();
                console.log(errors);
                displayValidationErrors(errors);
            } else {
                console.error('Ошибка при обновлении сотрудника');
            }
        } else {
            modal.hide();
            await loadEmployees();
            document.getElementById('employeeModal').remove();
        }
    } catch (error) {
        console.error('Ошибка при обновлении сотрудника:', error);
    }
}


async function loadProjects(){
    try {
        const response = await fetch(apiProject);
        const data = await response.json();
        console.log('Projects', data);
        const projectBlock = document.querySelector('.projectBlock');
        projectBlock.innerHTML = `
            <button class="btn btn-primary mb-3" onclick="showCreateProjectModal()">Создать проект</button>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>Название</th>
                        <th>Название компании заказчика</th>
                        <th>Название компании исполнителя</th>
                        <th>Имя руководителя проекта</th>
                        <th>Приоритет</th>
                        <th>Дата начала</th>
                        <th>Дата конца</th>
                        <th>Исполнители</th>
                        <th>Действия</th>
                    </tr>
                </thead>
                <tbody>
                    ${data.map(project => `
                        <tr>
                            <td>${project.name}</td>
                            <td>${project.customerCompanyName}</td>
                            <td>${project.executorCompanyName}</td>
                            <td>${project.projectManagerName}</td>
                            <td>${project.priority}</td>
                            <td>${project.startDate ? new Date(project.startDate).toLocaleDateString() : ''}</td>
                            <td>${project.endDate ? new Date(project.endDate).toLocaleDateString() : ''}</td>
                            <td>
                            <div>
                                ${project.projectEmployees.length > 0 
                                ? project.projectEmployees.map(emp => `${emp.lastName} ${emp.firstName} ${emp.middleName || ''}
                                    <button class="btn btn-sm btn-danger" onclick="removeEmployeeFromProject(${project.id}, ${emp.id})">Удалить</button>
                                `).join('<br>')
                                : 'Нет исполнителей'}
                               
                            </div>
                                <button class="btn btn-sm btn-success" onclick="showAddEmployeeToProjectModal(${project.id})">Добавить</button>
                            </td>
                            <td>
                                <button class="btn btn-warning btn-sm" onclick="editProject(${project.id})">Редактировать</button>
                                <button class="btn btn-danger btn-sm" onclick="deleteProject(${project.id})">Удалить</button>
                            </td>
                        </tr>
                    `).join('')}
                </tbody>
            </table>
        `;
    }catch (error) {
        console.log('Ошибка при получении проектов', error);
    }
}

async function showAddEmployeeToProjectModal(projectId) {
    const employees = await fetch(apiEmployee).then(res => res.json());

    const modalHtml = `
            <div class="modal fade" id="addEmployeeModal" tabindex="-1" aria-labelledby="addEmployeeModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Добавить сотрудника в проект</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                        </div>
                        <div class="modal-body">
                            <form id="addEmployeeForm">
                                <div class="mb-3">
                                    <label class="form-label">Выберите сотрудника</label>
                                    <select class="form-select" id="employeeId" required>
                                        ${employees.map(e => `<option value="${e.id}">${e.lastName} ${e.firstName}</option>`).join('')}
                                    </select>
                                </div>
                                <button type="submit" class="btn btn-primary">Добавить</button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        `;

    document.body.insertAdjacentHTML('beforeend', modalHtml);
    const modal = new bootstrap.Modal(document.getElementById('addEmployeeModal'));
    modal.show();

    document.getElementById('addEmployeeForm').onsubmit = async (e) => {
        e.preventDefault();
        const employeeId = document.getElementById('employeeId').value;
        await addEmployeeToProject(projectId, employeeId, modal);
    };
}
async function addEmployeeToProject(projectId, employeeId, modal) {
    try {
        const response = await fetch(`${apiProjectEmployee}/add?projectId=${projectId}&employeeId=${employeeId}`, {
            method: 'POST'
        });

        if (!response.ok) {
            if (response.status === 400) {
                const errors = await response.json();
                console.log(errors);
                alert('Сотрудник уже является исполнителем этого проекта')
            } else {
                throw new Error('Ошибка при добавлении сотрудника в проект');
            }
        } else {
            modal.hide();
            await loadProjects();
            document.getElementById('addEmployeeModal').remove();
        }
    } catch (error) {
        console.error('Ошибка при добавлении сотрудника в проект:', error);
    }
}

async function removeEmployeeFromProject(projectId, employeeId) {
    console.log(`Удаление сотрудника из проекта: projectId=${projectId}, employeeId=${employeeId}`);
    try {
        const response = await fetch(`${apiProjectEmployee}/delete?projectId=${projectId}&employeeId=${employeeId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            if (response.status === 400) {
                const errors = await response.json();
                console.log(errors);
                alert('Ошибка при удалении сотрудника из проекта');
            } else {
                throw new Error('Ошибка при удалении сотрудника из проекта');
            }
        } else {
            await loadProjects();  
        }
    } catch (error) {
        console.error('Ошибка при удалении сотрудника из проекта:', error);
    }
}

async function deleteProject(id) {
    if(confirm("Вы уверены что хотите удалить этот проект?")){
        try {
            const response = await fetch(`${apiProject}/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) {
                console.error('Ошибка при удалении проекта');
            } else {
                await loadProjects();
            }
        }catch (error) {
            console.error('Ошибка при удалении проекта:', error);
        }
    }
}

async function createProject(modal) {
    clearValidationErrors();

    const project = {
        name: document.getElementById('ProjectName').value,
        startDate: document.getElementById('StartDate').value,
        endDate: document.getElementById('EndDate').value,
        priority: document.getElementById('Priority').value,
        customerCompanyId: document.getElementById('CustomerCompanyId').value,
        executorCompanyId: document.getElementById('ExecutorCompanyId').value,
        projectManagerId: document.getElementById('ProjectManagerId').value
    };

    try {
        const response = await fetch(apiProject, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(project)
        });

        if (!response.ok) {
            if (response.status === 400) {
                const errors = await response.json();
                console.log(errors);
                displayValidationErrors(errors);
            } else {
                throw new Error('Ошибка при создании проекта');
            }
        } else {
            modal.hide();
            await loadProjects();
            document.getElementById('projectModal').remove();
        }
    } catch (error) {
        console.error('Ошибка при создании проекта:', error);
    }
}

async function showCreateProjectModal() {
    const companies = await fetch(apiCompany).then(response => response.json());
    const employees = await fetch(apiEmployee).then(response => response.json());
    const modalHtml = `
            <div class="modal fade" id="projectModal" tabindex="-1" aria-labelledby="projectModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Создать проект</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                        </div>
                        <div class="modal-body">
                            <form id="projectForm">
                                <div class="mb-3">
                                    <label class="form-label">Название проекта</label>
                                    <input type="text" class="form-control" id="ProjectName" required>
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Компания-заказчик</label>
                                    <select class="form-select" id="CustomerCompanyId" required>
                                        ${companies.map(c => `<option value="${c.id}">${c.name}</option>`).join('')}
                                    </select>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Компания-исполнитель</label>
                                    <select class="form-select" id="ExecutorCompanyId" required>
                                        ${companies.map(c => `<option value="${c.id}">${c.name}</option>`).join('')}
                                    </select>
                                </div>
                               <div class="mb-3">
                                    <label class="form-label">Руководитель проекта</label>
                                    <select class="form-select" id="ProjectManagerId" required>
                                        ${employees.map(e => `<option value="${e.id}">${e.lastName} ${e.firstName}</option>`).join('')}
                                    </select>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Дата начала</label>
                                    <input type="date" class="form-control" id="StartDate" required>
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Дата окончания</label>
                                    <input type="date" class="form-control" id="EndDate" required>
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Приоритет</label>
                                    <select class="form-select" id="Priority" required>
                                        <option value="1">Низкий</option>
                                        <option value="2">Средний</option>
                                        <option value="3">Высокий</option>
                                    </select>
                                </div>
                                <button type="submit" class="btn btn-primary">Создать</button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        `;

    document.body.insertAdjacentHTML('beforeend', modalHtml);
    const modal = new bootstrap.Modal(document.getElementById('projectModal'));
    modal.show();

    document.getElementById('projectForm').onsubmit = async (e) => {
        e.preventDefault();
        clearValidationErrors();
        await createProject(modal);
    };
}
const formatDateForInput = (isoString) => {
    if (!isoString) return '';
    return isoString.split('T')[0]; // Берём только дату, без времени и зоны
};


async function editProject(id) {
    try {
        const companies = await fetch(apiCompany).then(response => response.json());
        const employees = await fetch(apiEmployee).then(response => response.json());
        const project = await fetch(`${apiProject}/${id}`).then(response => response.json());
        console.log('Project by id',project);
        
        removeExistingModal('projectModal');

        const modalHtml = `
            <div class="modal fade" id="projectModal" tabindex="-1" aria-labelledby="projectModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Редактировать проект</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                        </div>
                        <div class="modal-body">
                            <form id="projectForm">
                                <div class="mb-3">
                                    <label class="form-label">Название проекта</label>
                                    <input type="text" class="form-control" id="ProjectName" value="${project.name}" required>
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Компания-заказчик</label>
                                    <select class="form-select" id="CustomerCompanyId" required>
                                        ${companies.map(c => `<option value="${c.id}" ${project.customerCompanyId === c.id ? 'selected' : ''}>${c.name}</option>`).join('')}
                                    </select>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Компания-исполнитель</label>
                                    <select class="form-select" id="ExecutorCompanyId" required>
                                        ${companies.map(c => `<option value="${c.id}" ${project.executorCompanyId === c.id ? 'selected' : ''}>${c.name}</option>`).join('')}
                                    </select>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Руководитель проекта</label>
                                    <select class="form-select" id="ProjectManagerId" required>
                                        ${employees.map(e => `<option value="${e.id}" ${project.projectManagerId === e.id ? 'selected' : ''}>${e.lastName} ${e.firstName}</option>`).join('')}
                                    </select>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Дата начала</label>
                                    <input type="date" class="form-control" id="StartDate" value="${formatDateForInput(project.startDate)}" required>
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Дата окончания</label>
                                    <input type="date" class="form-control" id="EndDate" value="${formatDateForInput(project.endDate)}" required>
                                    <div class="validation-error text-danger"></div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Приоритет</label>
                                    <select class="form-select" id="Priority" required>
                                        <option value="1" ${project.priority === 1 ? 'selected' : ''}>Низкий</option>
                                        <option value="2" ${project.priority === 2 ? 'selected' : ''}>Средний</option>
                                        <option value="3" ${project.priority === 3 ? 'selected' : ''}>Высокий</option>
                                    </select>
                                </div>
                                <button type="submit" class="btn btn-primary">Сохранить</button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
            `;
        document.body.insertAdjacentHTML('beforeend', modalHtml);
        const modal = new bootstrap.Modal(document.getElementById('projectModal'));
        modal.show();

        document.getElementById('projectForm').onsubmit = async (e) => {
            e.preventDefault();
            clearValidationErrors();
            await updateProject(id, modal);
        };
        
    }catch (error) {
        console.log("Ошибка при обновлении проекта", error);
    }
}

async function updateProject(id, modal) {
    const updatedProject = {
        Name: document.getElementById('ProjectName').value,
        StartDate: document.getElementById('StartDate').value,
        EndDate: document.getElementById('EndDate').value,
        Priority: parseInt(document.getElementById('Priority').value),
        CustomerCompanyId: parseInt(document.getElementById('CustomerCompanyId').value),
        ExecutorCompanyId: parseInt(document.getElementById('ExecutorCompanyId').value),
        ProjectManagerId: parseInt(document.getElementById('ProjectManagerId').value),
    }
    console.log('Updated Project:', updatedProject);
    try {
        const response = await fetch(`${apiProject}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedProject)
        });
        if (!response.ok) {
            if (response.status === 400) {
                const errors = await response.json();
                console.log(errors);
                displayValidationErrors(errors);
            } else {
                console.error('Ошибка при обновлении проекта', response);
            }
        }else{
            modal.hide();
            await loadProjects();
            document.getElementById('employeeModal').remove();
        }
        
    }catch(error) {
        console.log('Ошибка при обновлении проекта', error);
    }
}





function clearValidationErrors() {
    document.querySelectorAll('.validation-error').forEach(el => el.textContent = '');
}

function displayValidationErrors(errors) {
    if (errors && errors.errors) {
        Object.keys(errors.errors).forEach(key => {
            const errorMessages = errors.errors[key];
            const field = document.querySelector(`#${key}`);
            if (field) {
                const errorElement = field.nextElementSibling;
                if (errorElement) {
                    errorElement.textContent = errorMessages.join(', ');
                }
            }
        });
    }
}

function removeExistingModal(modalId) {
    const existingModal = document.getElementById(modalId);
    if (existingModal) {
        existingModal.remove();
    }
}






